using System.ComponentModel.DataAnnotations;

namespace VocalSpace.Models.Test
{
    public class ShareModalViewModel
    {
        public string? Title { get; set; }  // 彈窗標題
        public string? EventName { get; set; }  // 活動名稱
        public string? Date { get; set; }  // 活動日期
        public string? Location { get; set; }  // 活動地點
        public string? ImageUrl { get; set; }  // 活動封面圖片
        public string? Link { get; set; }  // 活動分享連結
        public string? CustomContent { get; set; }  // 自訂 HTML 內容
    }
}
