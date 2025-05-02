using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BookStore.Persistence.Managers.Books;
public class BookFileManager : IBookFileManager
{
    private readonly IWebHostEnvironment _env;

    public BookFileManager(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return;

        // Əgər path "/" ilə başlayırsa, onu təmizlə
        if (relativePath.StartsWith("/"))
            relativePath = relativePath.Substring(1);

        // Tam yol (wwwroot daxilində)
        string fullPath = Path.Combine(_env.WebRootPath, relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task<string> UploadSingleFileAsync(IFormFile file, string folder)
    {
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

        // Return relative paths
        return (
            pdfPath: $"/uploads/books/{Path.GetFileName(pdfPath)}",
            imagePath: $"/uploads/books/{Path.GetFileName(imagePath)}"
        );
    }
}

