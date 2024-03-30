using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation 
{
	public class FileService : IFileService
	{
        private readonly IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", "jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public Tuple<int, string> UpdateImage(string existingFileName, IFormFile newImageFile)
        {
            try
            {
                // Construct the path of the existing image file
                var wwwPath = this.environment.WebRootPath;
                var existingFilePath = Path.Combine(wwwPath, "Uploads", existingFileName);

                // Check if the existing file exists
                if (!System.IO.File.Exists(existingFilePath))
                {
                    return new Tuple<int, string>(0, "The specified image file does not exist.");
                }

                // Validate and create the new file path
                var newExt = Path.GetExtension(newImageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(newExt))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + newExt;
                var newFilePath = Path.Combine(wwwPath, "Uploads", newFileName);

                // Copy the content of the existing file to the new file
                System.IO.File.Copy(existingFilePath, newFilePath);

                // Delete the existing file
                System.IO.File.Delete(existingFilePath);

                // Save the content of the new image file
                using (var stream = new FileStream(newFilePath, FileMode.Append))
                {
                    newImageFile.CopyTo(stream);
                }

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occurred during the update process.");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
