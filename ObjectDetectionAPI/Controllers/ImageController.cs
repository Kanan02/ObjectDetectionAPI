using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObjectDetectionAPI.Dtos.RequestDtos;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Services;
using System.Security.Claims;

namespace ObjectDetectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ImageController : ControllerBase
    {
        private readonly FileStoreService _fileStoreService;
        private readonly PythonRunService _pythonRunService;

        public ImageController(FileStoreService fileStoreService,PythonRunService pythonRunService)
        {
            _fileStoreService = fileStoreService;
            _pythonRunService = pythonRunService;

        }

        [HttpPost("upload-image-content")]
        public async Task<IActionResult> AddImage(CreateFileStoreRequestDto request)
        {
            var response = await _fileStoreService.SaveImageInfo(request);
            if (response.Status == "Success")
                return StatusCode(StatusCodes.Status200OK, response);

            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

        [HttpPost("set-metadata")]
        public async Task<IActionResult> SetImageMetadta(SetFileStoreMetadataRequestDto request)
        {
            var response = await _fileStoreService.SetImageMetadata(request);
            if (response.Status == "Success")
                return StatusCode(StatusCodes.Status200OK, response);

            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(string id)
        {
            var response = await _fileStoreService.GetImage(id);
            if (response == null)
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                    {
                        Message = "Image id not found!",
                        Status = "Error"
                    });
            return Ok(response);
        }

        [HttpPost("upload-image")] 
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.UserId = userId;
            _pythonRunService.Run();
            var response = await _fileStoreService.UploadImage(request);
            if (response.Status == "Success")
                return StatusCode(StatusCodes.Status200OK, response);

            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

    }
}
