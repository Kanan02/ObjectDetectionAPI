using ObjectDetectionAPI.Models.Image;
using System.ComponentModel.DataAnnotations;

namespace ObjectDetectionAPI.Dtos.ResponseDtos
{
    public class FileStoreResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
        public string Path { get; set; }
        public string ProjectPath { get; set; }
        public List<Metadata> Metadatas { get; set; } = new List<Metadata>();
    }
}
