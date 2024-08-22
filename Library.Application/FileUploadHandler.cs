using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class FileUploadHandler
{
    public string Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return "NoImage.jpg";
        }
        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");

        using FileStream stream = new FileStream(uploadsFolder  + fileName, FileMode.Create);
        file.CopyTo(stream);

        return Path.Combine(fileName);
    }
}