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

        /// <summary>
        /// 上傳活動封面 
        /// </summary>
        public async Task<string> UploadActivityCover(IFormFile file)
        {
            return await UploadFile(file, "Activity");
        }

        /// <summary>
        /// 上傳User頭像 
        /// </summary>
        public async Task<string> UploadUserAvatar(IFormFile file)
        {
            return await UploadFile(file, "Avatar");
        }

        /// <summary>
        /// 上傳User頭像 
        /// </summary>
        public async Task<string> UploadUserBanner(IFormFile file)
        {
            return await UploadFile(file, "Avatar");
        }

        /// <summary>
        /// 
        /// </summary>

        private async Task<string> UploadFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("無效的檔案");

            // 檢查檔案大小 (限制 2MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
                throw new Exception("檔案大小不可超過 2MB");

            // 確保目錄存在
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image", folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 生成檔案名稱
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            // 如果檔案名稱重複，則直接使用現有的檔案
            if (!File.Exists(filePath))
            {
                // 儲存檔案
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            // 回傳相對路徑，存入資料庫
            return $"/image/{folderName}/{fileName}";
        }

        public async Task<string> UploadAudioFileAsync(IFormFile audioFile)
        {
            Console.WriteLine("fileService - audioFile:" + audioFile);
            if (audioFile == null || audioFile.Length == 0)
            {
                throw new ArgumentException("請選擇音樂檔案");
            }

            // 檢查副檔名
            var extension = Path.GetExtension(audioFile.FileName);
            if (extension.ToLower() != ".mp3")
            {
                throw new ArgumentException("僅允許 MP3 檔案");
            }

            // 限制檔案大小（30MB）
            if (audioFile.Length > 30 * 1024 * 1024)
            {
                throw new ArgumentException("檔案大小不可超過 30MB");
            }

            // 儲存檔案
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "audio");

            // 確保資料夾存在
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // 生成唯一檔名並儲存檔案
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(fileStream);
            }

            return $"/audio/{uniqueFileName}";
        }

        // 上傳封面圖片
        public async Task<string> UploadSongCoverAsync(IFormFile coverImage)
        {
            Console.WriteLine("fileService - coverImage:" + coverImage);
            if (coverImage == null || coverImage.Length == 0)
            {
                throw new ArgumentException("請選擇封面圖片");
            }

            var extension = Path.GetExtension(coverImage.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                throw new ArgumentException("僅允許 JPG、JPEG、PNG 圖片");
            }

            if (coverImage.Length > 2 * 1024 * 1024) // 限制 2MB
            {
                throw new ArgumentException("封面圖片大小不可超過 2MB");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image", "Song");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await coverImage.CopyToAsync(fileStream);
            }

            return $"/image/Song/{uniqueFileName}"; // 回傳相對路徑
        }
    }
}
