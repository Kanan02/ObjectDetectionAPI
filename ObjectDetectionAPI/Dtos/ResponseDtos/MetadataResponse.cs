using ObjectDetectionAPI.Models.Image;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObjectDetectionAPI.Dtos.ResponseDtos
{
    public class MetadataResponse
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FramedImage { get; set; }
        public string Details { get; set; }
    }
}
