using System.ComponentModel.DataAnnotations;

namespace VocalSpace.Models.Test
{

    //測試 公用彈窗及Activity model資料
    public class Activity
    {
        public string? EventCoverPath { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? EventTime { get; set; }  

        public string Location { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string? ActivityDescription { get; set; }
    }
}
