using AutoMapper;
using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Domain.Entities.Authors;
using BookStore.Domain.Entities.Books;
using BookStore.Domain.Entities.Categories;
using BookStore.Domain.Entities.Users;
using FluentValidation;

namespace BookStore.Persistence.Managers.Books;
public class BookManager : IBookManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Book> _baseManager;
    private readonly IValidator<CreateBookDto> _createValidator;
    private readonly IValidator<UpdateBookDto> _updateValidator;
    private readonly IEmailManager _emailManager;
    private readonly IBookFileManager _fileManager;

    public BookManager(
        IMapper mapper,
        IBaseManager<Book> baseManager,
        IValidator<CreateBookDto> createValidator,
        IValidator<UpdateBookDto> updateValidator,
        IEmailManager emailManager,
        IBookFileManager fileManager)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _emailManager = emailManager;
        _fileManager = fileManager;
    }

    public async Task<bool> CreateAsync(CreateBookDto dto)
    {
        await ValidateCreateDto(dto);

        var (pdfPath, imagePath) = await _fileManager.UploadBookFilesAsync(dto.PdfFile, dto.CoverImage);

        var book = _mapper.Map<Book>(dto);
        book.Path = pdfPath;
        book.CoverPath = imagePath;

        foreach (var categoryId in dto.CategoryIds)
        {
            book.BookCategories.Add(new BookCategory { CategoryId = categoryId });
        }

        foreach (var authorId in dto.AuthorIds)
        {
            book.Authors.Add(new BookAuthor { AuthorId = authorId });
        }

        await _baseManager.AddAsync(book);
        await _baseManager.Commit();

        // Email göndərmək
        var subscribers = await GetAllSubscribers();
        await _emailManager.SendEmailForSubscribers(subscribers, "Yeni kitab əlavə edildi", book.Title, book.Description);

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateBookDto dto)
    {
        await ValidateUpdateDto(dto);

        var book = await _baseManager.GetAsync(x => x.Id == dto.Id,"Authors", "BookCategories");
        if (book == null) throw new KeyNotFoundException("Book not found");

        _mapper.Map(dto, book);

        // Fayl dəyişibsə, əvvəlkini sil və yenisini yaz
        if (dto.PdfFile is not null)
        {
            _fileManager.DeleteFile(book.Path);
            var pdfPath = await _fileManager.UploadSingleFileAsync(dto.PdfFile, "books");
            book.Path = pdfPath;
        }

        if (dto.CoverImage is not null)
        {
            _fileManager.DeleteFile(book.CoverPath);
            var imagePath = await _fileManager.UploadSingleFileAsync(dto.CoverImage, "books");
            book.CoverPath = imagePath;
        }

        await _baseManager.SyncManyToMany(
     existingEntities: book.BookCategories,
     newEntities: dto.CategoryIds.Select(cid => new BookCategory
     {
         BookId = book.Id,
         CategoryId = cid
     }));

        await _baseManager.SyncManyToMany(
    existingEntities: book.Authors,
    newEntities: dto.AuthorIds.Select(aid => new BookAuthor
    {
        BookId = book.Id,
        AuthorId = aid
    }));
        book.IsDeleted = false;
        book.DeletedDate = null;
        book.UpdatedAt = DateTime.UtcNow;
        await _baseManager.Update(book);
        await _baseManager.Commit();
        return true;
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _baseManager.GetAsync(x => x.Id == id, nameof(Book.BookCategories), nameof(Book.Authors));
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _baseManager.GetAllAsync(null, nameof(Book.BookCategories), nameof(Book.Authors));
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _baseManager.GetAsync(x => x.Id == id);
        if (book == null)
            throw new KeyNotFoundException("Book not found.");

        book.DeletedDate = DateTime.UtcNow;
        book.IsDeleted = true;
        await _baseManager.Update(book);
        await _baseManager.Commit();
        return true;
    }

    #region Helpers
    private async Task ValidateCreateDto(CreateBookDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
    }

    private async Task ValidateUpdateDto(UpdateBookDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
    }

    private async Task<IEnumerable<User>> GetAllSubscribers()
    {
        return new List<User>();
    }
    #endregion
}

