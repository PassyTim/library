using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class FileUploadHandler
{
    public string Upload(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return "NoImage.jpg";
        }

        var fileName = GenerateFileName(file);
        var filePath = GenerateFilePath(fileName);

        using FileStream stream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(stream);

        return Path.Combine(fileName);
    }

    private string GenerateFileName(IFormFile file)
    {
        return Guid.NewGuid() + Path.GetExtension(file.FileName);
    }

    private string GenerateFilePath(string fileName)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/");
        return uploadsFolder + fileName;
    }
}