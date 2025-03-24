using Microsoft.AspNetCore.Hosting;

namespace VocalSpace.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadActivityCover(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("無效的檔案");

            // 檢查檔案大小 (限制 2MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
                throw new Exception("檔案大小不可超過 2MB");

            // 確保 wwwroot/image/Activity 目錄存在
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image", "Activity");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 生成唯一檔案名稱
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 儲存檔案
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 回傳相對路徑，存入資料庫
            return $"/image/Activity/{uniqueFileName}";
        }
    }
}
