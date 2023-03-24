using System.ComponentModel.DataAnnotations;

namespace ObjectDetectionAPI.Models.Image
{
    public class FileStore
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
        public string Path { get; set; }
        public string ProjectPath { get; set; }
        public string UserId { get; set; }
        public List<Metadata> Metadatas { get; set; } = new List<Metadata>();
    }
}
