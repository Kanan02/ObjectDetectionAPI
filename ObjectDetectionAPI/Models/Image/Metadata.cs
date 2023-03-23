using System.ComponentModel.DataAnnotations.Schema;

namespace ObjectDetectionAPI.Models.Image
{
    public class Metadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Image")]
        public string ImageId { get; set; }
        public FileStore Image { get; set; }

        public string FramedImage { get; set; }
        public string Details { get; set; }
    }
}
