namespace ObjectDetectionAPI.Dtos.RequestDtos
{
    public class UploadImageRequest
    {
        public IFormFile Image { get; set; }
        public string FolderDir { get; set; } = "";
        public string? UserId { get; set; } = "";
    }
}
