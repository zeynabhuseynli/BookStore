using AutoMapper;
using BookStore.Application.DTOs.Categories;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Domain.Entities.Categories;
using FluentValidation;

namespace BookStore.Persistence.Managers.Books;

public class CategoryManager : ICategoryManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Category> _baseManager;
    private readonly IValidator<CreateCategoryDto> _createValidator;
    private readonly IValidator<UpdateCategoryDto> _updateValidator;

    public CategoryManager(
        IMapper mapper,
        IBaseManager<Category> baseManager,
        IValidator<CreateCategoryDto> createValidator,
        IValidator<UpdateCategoryDto> updateValidator)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<bool> CreateAsync(CreateCategoryDto dto)
    {
        await ValidateCreateDto(dto);
        await EnsureCategoryNameIsUnique(dto.Name);

        var category = _mapper.Map<Category>(dto);

        if (dto.ParentCategoryId == 0)
        {
            category.ParentCategoryId = null;
        }
        else
        {
            var categ = await _baseManager.GetAsync(x=>x.Id==category.ParentCategoryId);
            if (categ == null)
                throw new  ValidationException("Not Found ParentCategoryId");
            category.ParentCategoryId = dto.ParentCategoryId;
        }
        await _baseManager.AddAsync(category);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
    {
        var category = await _baseManager.GetAsync(x => x.Id == dto.Id);
        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        await ValidateUpdateDto(dto);
        await EnsureCategoryNameIsUnique(dto.Name, dto.Id);
        _mapper.Map(dto, category);

        if (dto.ParentCategoryId == 0)
        {
            category.ParentCategoryId = null;
        }
        else
        {
            var categ = await _baseManager.GetAsync(x => x.Id == category.ParentCategoryId);
            if (categ == null)
                throw new ValidationException("Not Found ParentCategoryId");
            category.ParentCategoryId = dto.ParentCategoryId;
        }
        category.UpdatedAt = DateTime.UtcNow;
        await _baseManager.Update(category);
        await _baseManager.Commit();
        return true;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _baseManager.GetAsync(x => x.Id == id, nameof(Category.SubCategories));
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _baseManager.GetAllAsync(null, nameof(Category.SubCategories));
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _baseManager.GetAsync(x => x.Id == id, nameof(Category.SubCategories), nameof(Category.BookCategories));
        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        if (category.SubCategories != null && category.SubCategories.Any())
            throw new InvalidOperationException("Cannot delete category with subcategories.");

        if (category.BookCategories != null && category.BookCategories.Any())
            throw new InvalidOperationException("Cannot delete category linked to books.");

        category.DeletedDate = DateTime.UtcNow;
        category.IsDeleted = true;
        await _baseManager.Update(category);
        await _baseManager.Commit();
        return true;
    }

    #region Helpers
    private async Task ValidateCreateDto(CreateCategoryDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
    }

    private async Task ValidateUpdateDto(UpdateCategoryDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
    }

    private async Task EnsureCategoryNameIsUnique(string name, int id = 0)
    {
        bool isUnique = await _baseManager.IsPropertyUniqueAsync(x => x.Name, name, id);
        if (!isUnique)
        {
            throw new InvalidOperationException("Category name must be unique.");
        }
    }
    #endregion
}

