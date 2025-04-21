using CloudinaryDotNet.Actions;

namespace Cloudinary.Services
{
    public interface IImageService
    {
        public Task<ImageUploadResult?> UploadImageAsync(IFormFile file);
        public Task<DeletionResult> DeleteImageAsync(string publicId);
        public  Task<ImageUploadResult?> UpdateImageAsync(string publicId, IFormFile NewFile);
        public Task<GetResourceResult?> GetImageAsync(string publicId);
    }
}
