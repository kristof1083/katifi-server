namespace KatifiWebServer.Services
{
    public interface IImageFileService
    {
        public bool ValidImageFile(IFormFile file);

        public Task<bool> SaveImageAsync(IFormFile file, string folderName, string newFileName);
    }
}
