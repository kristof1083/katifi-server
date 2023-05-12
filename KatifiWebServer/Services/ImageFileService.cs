using Microsoft.AspNetCore.Http;

namespace KatifiWebServer.Services
{
    public class ImageFileService : IImageFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ImageFileService> _logger;

        public ImageFileService(IWebHostEnvironment environment, IConfiguration configuration, ILogger<ImageFileService> logger)
        {
            this._environment = environment;
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<bool> SaveImageAsync(IFormFile file, string folderName, string baseFileName)
        {
            try
            {
                if (file == null || !ValidImageFile(file))
                    throw new FileLoadException();

                var folderPath = Path.Combine(_environment.ContentRootPath, "Uploads", folderName);
                var fileName = this.SetFileName(folderPath, baseFileName);

                if (string.IsNullOrEmpty(fileName))
                    throw new IOException();

                fileName += Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName); 
                
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"'{file.FileName}' succesfull uploaded as '{fileName}'");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Can not upload: '{file.FileName}'");
                return false;
            }
        }

        public bool ValidImageFile(IFormFile file)
        {
            var permittedExtensions = _configuration.GetSection("FileUploads:permittedExtensions").Get<List<string>>();
            if(permittedExtensions == null)
                return false;

            var fi = new FileInfo(file.FileName);
            string extension = fi.Extension.ToLower();

            if (file == null || string.IsNullOrEmpty(file.FileName) || !permittedExtensions.Contains(extension))
                return false;

            return true;
        }

        private string? SetFileName(string folderPath, string baseFileName)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            int count = 0;
            var dr = new DirectoryInfo(folderPath);
            foreach(var fileinfo in dr.EnumerateFiles())
            {
                if (fileinfo.Name.Contains(baseFileName))
                {
                    count++;
                }  
            }

            if (count >= 20)
                return null;

            return $"{baseFileName}_{count + 1}";
        }

    }
}
