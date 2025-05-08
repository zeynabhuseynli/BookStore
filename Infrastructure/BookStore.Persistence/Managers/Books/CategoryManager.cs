using AutoMapper;
using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.DTOs.Categories;
using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Categories;
using BookStore.Infrastructure.BaseMessages;

namespace BookStore.Persistence.Managers.Books;
public class CategoryManager : ICategoryManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Category> _baseManager;
    private readonly IClaimManager _claimManager;

    public CategoryManager(IMapper mapper, IBaseManager<Category> baseManager, IClaimManager claimManager)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _claimManager = claimManager;
    }

    public async Task<bool> CreateAsync(CreateCategoryDto dto)
    {
        await _baseManager.ValidateAsync(dto);
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
                throw new  KeyNotFoundException(UIMessage.GetNotFoundMessage("ParentCategoryId"));
            category.ParentCategoryId = dto.ParentCategoryId;
        }
        await _baseManager.AddAsync(category, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
    {
        var category = await _baseManager.GetAsync(x => x.Id == dto.Id);
        if (category == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Category"));

        await _baseManager.ValidateAsync(dto);
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
                throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("ParentCategoryId"));
            category.ParentCategoryId = dto.ParentCategoryId;
        }
      
        _baseManager.Update(category, _claimManager.GetCurrentUserId());

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
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Category"));

        if (category.SubCategories != null && category.SubCategories.Any())
            throw new BadRequestException(UIMessage.CATEGORY_HAS_SUBCATEGORIES);

        if (category.BookCategories != null && category.BookCategories.Any())
            throw new BadRequestException(UIMessage.CATEGORY_LINKED_TO_BOOKS);

        _baseManager.Update(category, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<IEnumerable<CategoryDto>> GetSubCategoriesByCategoryIdAsync(int categoryId)
    {
        var category = await _baseManager.GetAsync(x => x.Id == categoryId, nameof(Category.SubCategories));

        if (category == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Category"));

        return _mapper.Map<IEnumerable<CategoryDto>>(category.SubCategories);
    }

    public async Task<IEnumerable<BookDto>> GetBooksBySubCategoryIdAsync(int subCategoryId)
    {
        var category = await _baseManager.GetAsync(x => x.Id == subCategoryId, nameof(Category.BookCategories), "BookCategories.Book");

        if (category == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("SubCategory"));
        
            var books = category.BookCategories
           .Select(bc => bc.Book)
           .Where(b => b != null)
           .ToList();

            return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<CategoryDto>> GetDeletedCategoriesAsync()
    {
        var categories = await _baseManager.GetAllAsync(
            x => x.IsDeleted,
            nameof(Category.SubCategories));

        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<bool> SoftDeleteCategoryWithDependenciesAsync(int categoryId)
    {
        var category = await _baseManager.GetAsync(x => x.Id == categoryId,
            nameof(Category.SubCategories),
            nameof(Category.BookCategories),
            nameof(Category.BookCategories) + "." + nameof(BookCategory.Book));

        if (category == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Category"));

        if (category.SubCategories != null)
        {
            foreach (var sub in category.SubCategories)
            {
                await SoftDeleteCategoryWithDependenciesAsync(sub.Id);
            }
        }

        if (category.BookCategories != null)
        {
            foreach (var bc in category.BookCategories)
            {
                var cat = bc.Category;
                if (cat!=null)
                {
                    _baseManager.SoftDelete(cat, _claimManager.GetCurrentUserId());
                    _baseManager.Update(cat);
                }
                var book = bc.Book;
                if (book != null)
                {
                    _baseManager.SoftDelete(book, _claimManager.GetCurrentUserId());
                    _baseManager.Update(book);
                }
            }
        }

        _baseManager.SoftDelete(category, _claimManager.GetCurrentUserId());
        _baseManager.Update(category);
        await _baseManager.Commit();

        return true;
    }

    public async Task<bool> RecoverCategoryWithDependenciesAsync(int categoryId)
    {
        var category = await _baseManager.GetAsync(x => x.Id == categoryId,
            nameof(Category.SubCategories),
            nameof(Category.BookCategories),
            nameof(Category.BookCategories) + "." + nameof(BookCategory.Book));

        if (category == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Category"));

        if (category.SubCategories != null)
        {
            foreach (var sub in category.SubCategories)
            {
                await RecoverCategoryWithDependenciesAsync(sub.Id);
            }
        }

        if (category.BookCategories != null)
        {
            foreach (var bc in category.BookCategories)
            {

                var cat = bc.Category;
                if (cat != null)
                {
                    _baseManager.Recover(cat);
                    _baseManager.Update(cat);
                }
                var book = bc.Book;
                if (book != null)
                {
                    _baseManager.Recover(book);
                    _baseManager.Update(book);
                }
            }
        }

        _baseManager.Recover(category);
        _baseManager.Update(category);
        await _baseManager.Commit();

        return true;
    }

    private async Task EnsureCategoryNameIsUnique(string name, int id = 0)
    {
        bool isUnique = await _baseManager.IsPropertyUniqueAsync(x => x.Name, name, id);
        if (!isUnique)
        {
            throw new BadRequestException(UIMessage.GetUniqueNamedMessage("Category name"));
        }
    }
}

