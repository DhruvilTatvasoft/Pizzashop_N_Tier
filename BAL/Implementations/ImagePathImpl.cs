using Microsoft.AspNetCore.Http;

public class imagePathImpl : IImagePath
{
    public string getImagePath(IFormFile file)
    {
       string? imagePath = null;
       if (file != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
               file.CopyTo(fileStream);
            }

            imagePath = $"/uploads/{uniqueFileName}";
        }
        return imagePath;
    }
}