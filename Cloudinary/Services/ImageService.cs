
using Cloudinary.Helper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;


namespace Cloudinary.Services
{
    public class ImageService : IImageService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary; // Correctly reference the Cloudinary class
        private readonly ILogger<ImageService> _logger;

        public ImageService(IOptions<CloudinarySetting> options,ILogger<ImageService> logger)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.APIKey,
                options.Value.APISecret
            );

            _cloudinary = new CloudinaryDotNet.Cloudinary(account); // Instantiate the Cloudinary object
            this._logger = logger;
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Result == "ok")
            {
                return result;
            }
            else
            {
                _logger.LogError($"Failed to delete image with publicId: {publicId}. Error: {result.Error}");
                return new DeletionResult
                {
                    Result = "error",
                    Error = result.Error
                };
            }

        }

        public async Task<GetResourceResult?> GetImageAsync(string publicId)
        {
            var getParams = new GetResourceParams(publicId);
           
            var result = await _cloudinary.GetResourceAsync(getParams);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }
            else
            {
                _logger.LogError($"Failed to get image with publicId: {publicId}. Error: {result.Error}");
                return null;
            }


        }

        public async Task<ImageUploadResult?> UpdateImageAsync(string publicId, IFormFile NewFile)
        {
            var updateParams = new ImageUploadParams
            {
                File = new FileDescription(NewFile.FileName, NewFile.OpenReadStream()),
                PublicId = publicId,
                Overwrite = true
            };
            var result = await _cloudinary.UploadAsync(updateParams);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }
            else
            {
                _logger.LogError($"Failed to update image with publicId: {publicId}. Error: {result.Error}");
                return null;
            }

        }

        public async Task<ImageUploadResult?> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult;
            }
            else
            {
                _logger.LogError($"Faild to upLoad Result");
                return null;
            }
        }
    }
}
