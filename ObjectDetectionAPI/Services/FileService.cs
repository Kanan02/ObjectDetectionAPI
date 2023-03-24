using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using ObjectDetectionAPI.Utilities;

namespace ObjectDetectionAPI.Services
{
    public class PathResponse
    {
        public string PhysicalPath { get; set; }
        public string VirtualPath { get; set; }
    }
    public class FileService
    {
        private readonly string _uploadDirectory;
        private readonly string _developmentDirectory;

        public FileService(IWebHostEnvironment environment)
        {
            var isProduction = environment.IsProduction();

            //tests only
            //_isProduction = true;
            _developmentDirectory = environment.ContentRootPath;
            var productionDirectory = environment.ContentRootPath;
            _uploadDirectory = Path.Combine(isProduction ? productionDirectory : _developmentDirectory, "Uploads");

        }

        public async Task<PathResponse> SaveAsync(string folderPath, string fileName, Stream fileStream)
        {
            fileStream.Position = 0;
            var folderDir = Path.Combine(_uploadDirectory, folderPath);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = Utils.GenerateRandomFileName(fileNameWithoutExtension, fileExtension);

            var path = Path.Combine(folderDir, uniqueFileName);
            CreateIfMissing(path);

            await using var stream = File.Create(path);
            await fileStream.CopyToAsync(stream).ConfigureAwait(false);
            return new PathResponse()
            {
                PhysicalPath = path,
                VirtualPath = ResolveVirtual(path),
            };

        }
        public async Task<PathResponse> SaveAsync(string folderPath, IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var result = await SaveAsync(folderPath, file.FileName, stream);
            return result;
        }

        public Stream GetFile(string path)
        {
            return new FileStream(path ?? string.Empty, FileMode.Open, FileAccess.ReadWrite);
        }
        public string ResolveVirtual(string physicalPath)
        {

            string url = physicalPath.Substring(this._developmentDirectory.Length).Replace('\\', '/');
            return (url);
        }
        public void CreateIfMissing(string path)
        {
            path = Path.GetDirectoryName(path);

            var folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);

        }

        public void Delete(params string[] paths)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
