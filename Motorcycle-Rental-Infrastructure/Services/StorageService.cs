using Microsoft.AspNetCore.Hosting;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _basePath;

        public StorageService(IWebHostEnvironment env)
        {
            _basePath = Path.Combine(env.ContentRootPath, "uploads");
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task SaveFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            if (extension != ".png" && extension != ".bmp")
                throw new InvalidOperationException("Formato inválido. Apenas PNG ou BMP são aceitos.");

            var path = Path.Combine(_basePath, fileName);
            using var fileStreamOut = new FileStream(path, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOut);
        }
    }
}
