using AutoMapper;
using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Domain.Entities.Authors;
using BookStore.Infrastructure.BaseMessages;
using FluentValidation;

namespace BookStore.Persistence.Managers.Books;
public class AuthorManager: IAuthorManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Author> _baseManager;
    private readonly IValidator<CreateAuthorDto> _createAuthorValidator;
    private readonly IValidator<UpdateAuthorDto> _updateAuthorValidator;

    public AuthorManager(IMapper mapper, IBaseManager<Author> baseManager, IValidator<CreateAuthorDto> createAuthorValidator, IValidator<UpdateAuthorDto> updateAuthorValidator)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _createAuthorValidator = createAuthorValidator;
        _updateAuthorValidator = updateAuthorValidator;
    }

    public async Task<bool> CreateAsync(CreateAuthorDto dto)
    {
        await ValidateCreateAuthorDto(dto);
        await EnsureAuthorDoesNotExist(dto);

        var author = _mapper.Map<Author>(dto);
        await _baseManager.AddAsync(author);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateAuthorDto dto)
    {
        var author = await _baseManager.GetAsync(x=>x.Id==dto.Id);
        if (author == null) throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Author"));

        await ValidateUpdateAuthorDto(dto);
        await EnsureAuthorDoesNotExist(dto);

        _mapper.Map(dto, author);
        author.UpdatedAt = DateTime.UtcNow;
        await _baseManager.Update(author);
        await _baseManager.Commit();
        return true;
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await _baseManager.GetAsync(x=>x.Id==id);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        var authors = await _baseManager.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }

    #region Private Helper Methods

    private async Task ValidateCreateAuthorDto(CreateAuthorDto dto)
    {
        var validationResult = await _createAuthorValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new Application.Exceptions.ValidationException();
        }
    }

    private async Task ValidateUpdateAuthorDto(UpdateAuthorDto dto)
    {
        var validationResult = await _updateAuthorValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new Application.Exceptions.ValidationException();
        }
    }

    private async Task EnsureAuthorDoesNotExist(CreateAuthorDto dto, int? id=0)
    {
        Author author = await _baseManager
            .GetAsync(x => x.FirstName == dto.FirstName && x.LastName == dto.LastName && x.BirthDay==dto.BirthDay&&x.Id==id);

        if (author!=null)
        {
            throw new BadRequestException(UIMessage.GetUniqueNamedMessage("Author"));
        }
    }
    #endregion
}

