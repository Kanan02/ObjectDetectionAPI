using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public FileStoreService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _context= dbContext;
            _userManager = userManager;
        }

        public async Task<Response> SaveImageInfo(CreateFileStoreRequestDto fileStoreDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(fileStoreDto.UserId);
                if (user == null)
                {
                    return new Response()
                    {
                        Status = "Error",
                        Message = "User does not exist!"
                    };
                }

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
                    UserId = fileStoreDto.UserId,
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
            var image = await _context.FileStores.Include("Metadatas").FirstOrDefaultAsync(x=>x.Id==id);
            if (image == null)
                return null;

            List<MetadataResponse> metatadasResponse = new List<MetadataResponse>();
            foreach (var item in image.Metadatas)
            {
                metatadasResponse.Add(new MetadataResponse()
                {
                    Details = item.Details,
                    FramedImage = item.FramedImage,
                    Id = item.Id,
                });
            }

            return new FileStoreResponseDto()
            {
                FileExtension = image.FileExtension,
                ContentType = image.ContentType,
                SizeInBytes = image.SizeInBytes,
                FileName = image.FileName,
                Id = image.Id,
                Metadatas = metatadasResponse,
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

        public async Task<IEnumerable<FileStore>> GetImagesByUserId(string userId)
        {
            var images = await _context.FileStores.Include("Metadatas").Where(x => x.UserId == userId).ToListAsync();
            return images;

        }
    }
}
