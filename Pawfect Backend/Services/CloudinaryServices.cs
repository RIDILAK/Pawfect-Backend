using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace Pawfect_Backend.Services
{
    public interface ICLoudinaryServices
    {
        Task <string> UploadImage(IFormFile  file);
    }
    public class CloudinaryServices:ICLoudinaryServices
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        } 

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "pawfect_uploads"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

    }
}
