using AutoMapper;
using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Authors;
using BookStore.Domain.Entities.Books;
using BookStore.Domain.Entities.Categories;
using BookStore.Domain.Entities.Enums;
using BookStore.Infrastructure.BaseMessages;

namespace BookStore.Persistence.Managers.Books;
public class BookManager : IBookManager
{
    private readonly IMapper _mapper;
    private readonly IBaseManager<Book> _baseManager;
    private readonly IEmailManager _emailManager;
    private readonly IClaimManager _claimManager;
    private readonly IUserManager _userManager;
    private readonly IBookFileManager _fileManager;

    public BookManager(
        IMapper mapper,
        IBaseManager<Book> baseManager,
        IClaimManager claimManager,
        IEmailManager emailManager,
        IBookFileManager fileManager,
        IUserManager userManager)
    {
        _mapper = mapper;
        _baseManager = baseManager;
        _claimManager = claimManager;
        _emailManager = emailManager;
        _fileManager = fileManager;
        _userManager = userManager;
    }

    public async Task<bool> CreateAsync(CreateBookDto dto)
    {
        await _baseManager.ValidateAsync(dto);

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

        await _baseManager.AddAsync(book,_claimManager.GetCurrentUserId());
        await _baseManager.Commit();

        // Email göndərmək
        var subscribers = await _userManager.GetAllSubscribers();
        await _emailManager.SendEmailForSubscribers(subscribers, UIMessage.ADD_MESSAGE, book.Title, book.Description);

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateBookDto dto)
    {
        await _baseManager.ValidateAsync(dto);

        var book = await _baseManager.GetAsync(x => x.Id == dto.Id, "Authors", "BookCategories");
        if (book == null) throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        _mapper.Map(dto, book);

        if (dto.PdfFile is not null)
        {
            if (!string.IsNullOrWhiteSpace(book.Path))
                _fileManager.DeleteFile(book.Path);

            var newPdfPath = await _fileManager.UploadSingleFileAsync(dto.PdfFile, "books");
            book.Path = newPdfPath;
        }

        if (dto.CoverImage is not null)
        {
            if (!string.IsNullOrWhiteSpace(book.CoverPath))
                _fileManager.DeleteFile(book.CoverPath);

            var newCoverPath = await _fileManager.UploadSingleFileAsync(dto.CoverImage, "books");
            book.CoverPath = newCoverPath;
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

        _baseManager.Update(book, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var book = await _baseManager.GetAsync(x => x.Id == id);
        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        _baseManager.SoftDelete(book, _claimManager.GetCurrentUserId());
        _baseManager.Update(book, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }
    
    public async Task<bool> RecoverBookAsync(int id)
    {
        var book = await _baseManager.GetAsync(x => x.Id == id);
        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        _baseManager.Recover(book);
        _baseManager.Update(book, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> HardDeleteAsync(int bookId)
    {
        var book = await _baseManager.GetAsync(
            x => x.Id == bookId,
            nameof(Book.Authors),
            nameof(Book.BookCategories)
        );

        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        _fileManager.DeleteFile(book.Path);
        _fileManager.DeleteFile(book.CoverPath);

        var authorLinks = book.Authors.ToList();
        _baseManager.HardRemoveRange(authorLinks);
        var categoryLinks = book.BookCategories.ToList();
        _baseManager.HardRemoveRange(categoryLinks);

        await _baseManager.Commit(); 

        _baseManager.HardDelete(book);
        await _baseManager.Commit();

        return true;
    }
    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _baseManager.GetAsync(x => x.Id == id, nameof(Book.BookCategories), nameof(Book.Authors));
        if (book!=null)
        {
            await IncrementViewCountAsync(book);
            return _mapper.Map<BookDto>(book);
        }
        return null;
    }
        
    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _baseManager.GetAllAsync(null, nameof(Book.BookCategories), nameof(Book.Authors));
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<int>> GetAuthorIdsByBookIdAsync(int bookId)
    {
        var book = await _baseManager.GetAsync(x => x.Id == bookId, nameof(Book.Authors), "Authors.Author");
        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        return book.Authors.Select(a => a.AuthorId);
    }

    public async Task<IEnumerable<int>> GetCategoryIdsByBookIdAsync(int bookId)
    {
        var book = await _baseManager.GetAsync(x => x.Id == bookId, nameof(Book.BookCategories));
        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        return book.BookCategories.Select(c => c.CategoryId);
    }

    public async Task<bool> SendBookPdfToEmailAsync(int bookId)
    {
        var user = await _userManager.GetCurrentUserAsync();
        if (user == null || !user.IsActivated)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("User"));

        var book = await _baseManager.GetAsync(x => x.Id == bookId);
        if (book == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Book"));

        if (string.IsNullOrWhiteSpace(book.Path))
            throw new FileNotFoundException("Kitabın PDF faylı mövcud deyil.");

        string fullPath = _fileManager.GetFullFilePath(book.Path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("PDF faylı tapılmadı.", fullPath);

        await _emailManager.SendPdfAsync(
            user.Email,
            $"📘 Kitab: {book.Title}",
            $"<p>Salam <strong>{user.FirstName}</strong>, istədiyiniz kitab əlavə olunub!</p>",
            fullPath
        );

        return true;
    }
    
    private async Task IncrementViewCountAsync(Book book)
    {
        var user = await _userManager.GetCurrentUserAsync();
        if (user!=null&&user.Role!=Role.Admin&&user.Role!=Role.SuperAdmin)
        {
            book.ViewCount += 1;

            _baseManager.Update(book);
            await _baseManager.Commit();
        }
    }
}

