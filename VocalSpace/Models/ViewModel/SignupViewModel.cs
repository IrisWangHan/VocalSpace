using System.ComponentModel.DataAnnotations;

namespace VocalSpace.Models.ViewModel
{
    public class SignupViewModel
    {
        // Step 1: 帳號 & 密碼
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string? SignupAccount { get; set; }

        [Required(ErrorMessage = "密碼為必填欄位")]
        [StringLength(28, MinimumLength = 6)]
        public string? SignupPassword { get; set; }  

        // Step 2: 電子信箱
        [Required]
        public string? SignupEmail { get; set; }  

        // Step 3: 驗證碼
        [Required]
        public string? SignupVerificationCode { get; set; }  

        // Step 4: 基本資料
        [Required]
        public string? SignupUserName { get; set; }  

        [Required]
        public DateOnly SignupUserBirthdate { get; set; }

        public string? SignupUserBio { get; set; }  
    }
}
