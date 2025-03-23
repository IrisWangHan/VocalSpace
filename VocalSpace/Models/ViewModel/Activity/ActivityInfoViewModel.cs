using VocalSpace.Models.ViewModel.Global;

namespace VocalSpace.Models.ViewModel.Activity
{
    public class ActivityInfoViewModel
    {
        //上傳者資訊
        public long UploaderId { get; set; }

        public string UploaderName { get; set; } = null!;

        public string UploaderAvatarPath { get; set; } = null!;

        //活動資訊
        public long ActivityId { get; set; }

        public string EventCoverPath { get; set; } = null!;

        public DateTime EventTime { get; set; }

        public string Title { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string City { get; set; } = null!;

        public byte ApprovalStatus { get; set; }

        public string ActivityDescription { get; set; } = null!;

        public CommentSectionViewModel CommentSection { get; set; } = new CommentSectionViewModel();

        public InterestedViewModel Interesteds { get; set; } = new InterestedViewModel();

        public string ShareUrl { get; set; } = null!;

    }
    public class InterestedViewModel
    {
        public bool IsInterested { get; set; }

        public long InterestedCount { get; set; }
    }
}
