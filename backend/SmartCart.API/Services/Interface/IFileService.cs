public interface IFileService
{
    Task<string> SaveFileAsync(
        IFormFile file,
        string folderName);

    Task DeleteFileAsync(
        string? filePath);
}