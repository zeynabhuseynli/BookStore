using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BookStore.Persistence.Managers.Books;
public class BookFileManager : IBookFileManager
{
    private readonly IWebHostEnvironment _env;

    public BookFileManager(IWebHostEnvironment env)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));

        if (string.IsNullOrWhiteSpace(_env.WebRootPath))
        {
            _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            Directory.CreateDirectory(_env.WebRootPath);
        }
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return;

        string fullPath = GetFullFilePath(relativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public async Task<string> UploadSingleFileAsync(IFormFile file, string folder)
    {
        if (file.Length > 10 * 1024 * 1024)
            throw new InvalidOperationException(UIMessage.GetFileTooLargeMessage(100));

        string folderPath = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{folder}/{Path.GetFileName(filePath)}";
    }

    public async Task<(string pdfPath, string imagePath)> UploadBookFilesAsync(IFormFile pdfFile, IFormFile coverImage)
    {
        if (pdfFile.Length > 100 * 1024 * 1024)
            throw new InvalidOperationException();

        string folderPath = Path.Combine(_env.WebRootPath, "uploads", "books");
        Directory.CreateDirectory(folderPath);

        string pdfPath = Path.Combine(folderPath, $"{Guid.NewGuid()}{Path.GetExtension(pdfFile.FileName)}");
        using (var stream = new FileStream(pdfPath, FileMode.Create))
        {
            await pdfFile.CopyToAsync(stream);
        }

        string imagePath = Path.Combine(folderPath, $"{Guid.NewGuid()}{Path.GetExtension(coverImage.FileName)}");
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await coverImage.CopyToAsync(stream);
        }

        return (
            pdfPath: $"/uploads/books/{Path.GetFileName(pdfPath)}",
            imagePath: $"/uploads/books/{Path.GetFileName(imagePath)}"
        );
    }

    public string GetFullFilePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException(UIMessage.FILE_PATH_NULL_OR_EMPTY(relativePath));

        if (Path.GetInvalidPathChars().Any(relativePath.Contains))
            throw new ArgumentException(UIMessage.FILE_PATH_INVALID(relativePath));

        if (relativePath.StartsWith("/"))
            relativePath = relativePath.Substring(1);

        return Path.Combine(_env.WebRootPath, relativePath);
    }
}

