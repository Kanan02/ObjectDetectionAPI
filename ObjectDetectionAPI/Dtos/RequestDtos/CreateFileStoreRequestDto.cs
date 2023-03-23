using System.ComponentModel.DataAnnotations;

namespace ObjectDetectionAPI.Dtos.RequestDtos
{
    public class CreateFileStoreRequestDto
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }
        [Required]
        [StringLength(8)]
        public string FileExtension { get; set; }
        [Required]
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string ProjectPath { get; set; }
    }
}
