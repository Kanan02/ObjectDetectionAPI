using System.Diagnostics;
using System.Text.Json;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Models.Image;
using static Community.CsharpSqlite.Sqlite3;

namespace ObjectDetectionAPI.Services
{
    public class PythonRunService
    {
        private readonly ApplicationDbContext _context;
        public PythonRunService(ApplicationDbContext dbContext)
        {
            _context= dbContext;
        }
        public async Task<Metadata> Run(string pathtoImage, string pathtoFolder,string imageId )
        {
            string cmd = "main.py";
            string args = $"{pathtoImage} {pathtoFolder}";
            
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/usr/bin/python";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string imageName = reader.ReadLine();
                    string details=reader.ReadToEnd();
                    var metadata = new Models.Image.Metadata() { Details = details, ImageId = imageId, FramedImage = imageName };
                    _context.Metadatas.Add(metadata);
                    await _context.SaveChangesAsync();
                    return metadata;
                }
            }

        }
    }
}