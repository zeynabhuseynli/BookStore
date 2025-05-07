using AutoMapper;
using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Authors;
using BookStore.Infrastructure.BaseMessages;

namespace BookStore.Persistence.Managers.Books;
public class AuthorManager : IAuthorManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Author> _baseManager;
    private readonly IClaimManager _claimManager;

    public AuthorManager(IMapper mapper, IBaseManager<Author> baseManager, IClaimManager claimManager)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _claimManager = claimManager;
    }

    public async Task<bool> CreateAsync(CreateAuthorDto dto)
    {
        await _baseManager.ValidateAsync(dto);
        await EnsureAuthorDoesNotExist(dto);

        var author = _mapper.Map<Author>(dto);
        await _baseManager.AddAsync(author, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateAuthorDto dto)
    {
        var author = await _baseManager.GetAsync(x => x.Id == dto.Id);
        if (author == null) throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Author"));

        await _baseManager.ValidateAsync(dto);
        await EnsureAuthorDoesNotExist(dto, dto.Id);

        _mapper.Map(dto, author);

        _baseManager.Update(author, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId)
    {
        var author = await _baseManager.GetAsync(
            x => x.Id == authorId,
            nameof(Author.Books) 
        );

        if (author == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Author"));

        var books = author.Books
            .Select(ba => ba.Book)
            .Where(book => book != null) 
            .ToList();

        return _mapper.Map<IEnumerable<BookDto>>(books);
    }


    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await _baseManager.GetAsync(x => x.Id == id, nameof(Author.Books));
        if (author == null)
            return null;

        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _baseManager.GetAllAsync(null, nameof(Author.Books));
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }

    private async Task EnsureAuthorDoesNotExist(CreateAuthorDto dto, int? id = 0)
    {
        Author author = await _baseManager.GetAsync(x => x.FirstName == dto.FirstName && x.LastName == dto.LastName && x.BirthDay == dto.BirthDay && x.Id != id);

        if (author != null)
            throw new BadRequestException(UIMessage.GetUniqueNamedMessage("Author"));
    }
}

