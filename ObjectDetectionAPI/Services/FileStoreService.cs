using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ObjectDetectionAPI.Dtos.RequestDtos;
using ObjectDetectionAPI.Dtos.ResponseDtos;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Models.Image;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
using ImageMagick;

namespace ObjectDetectionAPI.Services
{
    public class FileStoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FileService _fileService;

        public FileStoreService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, FileService fileService)
        {
            _context= dbContext;
            _userManager = userManager;
            _fileService = fileService;
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

        public async Task<FileStore> UploadImage(UploadImageRequest request)
        {
            PathResponse pathResponse;
            try
            {
                var file = request.Image;
                using var fileStream = new MemoryStream();
                await file.CopyToAsync(fileStream).ConfigureAwait(false);
                fileStream.Position = 0;

                using MagickImage image = new MagickImage(fileStream);
                image.Quality = 10; // This is the Compression level.
                image.Strip();
                using var newStream = new MemoryStream();
                await image.WriteAsync(newStream).ConfigureAwait(false);
                pathResponse = await this._fileService.SaveAsync(request.FolderDir, file.FileName, newStream);
            }
            catch (Exception e)
            {
                return new Response() { Status = "Error", Message = "Unable to save image!" };
            }

            var fileStore = new FileStore()
            {
                FileExtension = Path.GetExtension(request.Image.FileName),
                ContentType = request.Image.ContentType,
                FileName = request.Image.FileName,
                UniqueFileName = Path.GetFileName(pathResponse.PhysicalPath),
                Name = Path.GetFileNameWithoutExtension(request.Image.FileName),
                Path = pathResponse.PhysicalPath,
                ProjectPath = pathResponse.VirtualPath,
                SizeInBytes = request.Image.Length,
                UserId = request.UserId
            };
            await _context.FileStores.AddAsync(fileStore);
            await _context.SaveChangesAsync();
            return fileStore;
        }
    }
}
