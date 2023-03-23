using ObjectDetectionAPI.Models.Image;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObjectDetectionAPI.Dtos.RequestDtos
{
    public class SetFileStoreMetadataRequestDto
    {
        public string ImageId { get; set; }
        public string FramedImage { get; set; }
        public string Details { get; set; }
    }
}
