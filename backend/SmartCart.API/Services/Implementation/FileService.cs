using Microsoft.AspNetCore.Http;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    private readonly string[] _allowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png"
    };

    private const long MaxFileSize =
        5 * 1024 * 1024; // 5 MB


    public FileService(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }


    public async Task<string> SaveFileAsync(
        IFormFile file,
        string folderName)
    {
        if (file == null ||
            file.Length == 0)
        {
            throw new ArgumentException(
                "Invalid file.");
        }


        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException(
                "File size cannot exceed 5 MB.");
        }


        var extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();


        if (!_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException(
                "Only JPG, JPEG and PNG files are allowed.");
        }


        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName
        );


        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(
                uploadFolder);
        }


        var uniqueFileName =
            $"{Guid.NewGuid()}{extension}";


        var filePath = Path.Combine(
            uploadFolder,
            uniqueFileName
        );


        await using var stream =
            new FileStream(
                filePath,
                FileMode.Create
            );


        await file.CopyToAsync(stream);


        return $"/uploads/{folderName}/{uniqueFileName}";
    }


    public async Task DeleteFileAsync(
        string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }


        var fullPath = Path.Combine(
            _environment.WebRootPath,
            filePath.TrimStart(
                '/',
                '\\'
            )
        );


        if (File.Exists(fullPath))
        {
            await Task.Run(() =>
                File.Delete(fullPath));
        }
    }
}