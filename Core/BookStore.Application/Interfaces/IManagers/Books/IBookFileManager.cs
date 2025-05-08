using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface IBookFileManager
{
   // void DeleteFileIfExists(string? filePath);
    Task<(string pdfPath, string imagePath)> UploadBookFilesAsync(IFormFile pdfFile, IFormFile coverImage);
    void DeleteFile(string relativePath);
    Task<string> UploadSingleFileAsync(IFormFile file, string folder);
    string GetFullFilePath(string relativePath);
}

