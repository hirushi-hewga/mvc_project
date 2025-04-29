namespace mvc_project.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<string?> SaveImageAsync(IFormFile image, string path)
        {
            try
            {
                var types = image.ContentType.Split('/');

                if (types[0] != "image")
                {
                    return null;
                }
                
                string root = _webHostEnvironment.WebRootPath;
                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string imagePath = Path.Combine(root, path, imageName);

                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    using (var stream = image.OpenReadStream())
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteImage(string path)
        {
            string root = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(root, path);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
                return true;
            }

            return false;
        }
    }
}