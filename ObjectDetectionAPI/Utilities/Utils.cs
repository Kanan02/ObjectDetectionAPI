namespace ObjectDetectionAPI.Utilities
{
    public static class Utils
    {
        public static string GenerateRandomFileName(string filename, string fileExtension)
        {
            var uniquePrefix = "_" + Guid.NewGuid().ToString().Substring(0, 4);
            return $"{filename}_{uniquePrefix}{fileExtension}";
        }
    }
}
