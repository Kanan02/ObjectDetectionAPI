﻿using Microsoft.AspNetCore.Mvc;
using ObjectDetectionAPI.Dtos.RequestDtos;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Services;

namespace ObjectDetectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly FileStoreService _fileStoreService;

        public ImageController(FileStoreService fileStoreService)
        {
            _fileStoreService = fileStoreService;
        }

        [HttpPost]
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
    }
}
