using Cloudinary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cloudinary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            this._imageService = imageService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
      
            var result = await _imageService.UploadImageAsync(file);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return BadRequest("Public ID is required.");
            }
            var result = await _imageService.GetImageAsync(publicId);
            if (result != null)
            {
                return Ok(result.SecureUrl);
              //  var imageUrl = result.SecureUrl;
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetAsync(imageUrl);
                    
                //    var content = await response.Content.ReadAsByteArrayAsync();
                //    var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/png";

                //    return File(content, contentType);
                //}
            }
            else
            {
                return NotFound("Image not found.");
            }
        }
    }
}
