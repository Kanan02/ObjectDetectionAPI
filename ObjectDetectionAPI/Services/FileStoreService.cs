using Microsoft.EntityFrameworkCore;
using ObjectDetectionAPI.Dtos.RequestDtos;
using ObjectDetectionAPI.Dtos.ResponseDtos;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Models.Image;

namespace ObjectDetectionAPI.Services
{
    public class FileStoreService
    {
        private readonly ApplicationDbContext _context;
        public FileStoreService(ApplicationDbContext dbContext)
        {
            _context= dbContext;
        }

        public async Task<Response> SaveImageInfo(CreateFileStoreRequestDto fileStoreDto)
        {
            try
            {
                await _context.FileStores.AddAsync(new FileStore()
                {
                    FileExtension = fileStoreDto.FileExtension,
                    ContentType = fileStoreDto.ContentType,
                    FileName = fileStoreDto.FileName,
                    Name = fileStoreDto.Name,
                    Path = fileStoreDto.Path,
                    ProjectPath = fileStoreDto.ProjectPath,
                    SizeInBytes = fileStoreDto.SizeInBytes,
                    UniqueFileName = fileStoreDto.UniqueFileName,
                });

                await _context.SaveChangesAsync();
                return new Response()
                {
                    Status = "Success",
                    Message = "Image added successfully"
                };
            }
            catch (Exception)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "Cannot add image"
                };
            }
        }

        public async Task<FileStoreResponseDto?> GetImage(string id)
        {
            var image = await _context.FileStores.FirstOrDefaultAsync(x=>x.Id==id);
            if (image == null)
                return null;

            return new FileStoreResponseDto()
            {
                FileExtension = image.FileExtension,
                ContentType = image.ContentType,
                SizeInBytes = image.SizeInBytes,
                FileName = image.FileName,
                Id = image.Id,
                Metadatas = image.Metadatas,
                Name = image.Name,
                Path = image.Path,
                ProjectPath = image.ProjectPath,
                UniqueFileName = image.UniqueFileName,
            };
        }

        public async Task<Response> SetImageMetadata(SetFileStoreMetadataRequestDto request)
        {
            var image = await _context.FileStores.FirstOrDefaultAsync(x=>x.Id==request.ImageId);
            if (image == null)
                return new Response()
                {
                    Message = "Image id not found!",
                    Status = "Error"
                };

            await _context.Metadatas.AddAsync(new Metadata()
            {
                Details= request.Details,
                FramedImage= request.FramedImage,
                ImageId = request.ImageId,
            });

            await _context.SaveChangesAsync();
            return new Response()
            {
                Message = "Metadata added",
                Status = "Success"
            };
        }
    }
}
