namespace VocalSpace.Models.ViewModel.Global
{
    public class CommentViewModel
    {
        public long TypeId { get; set; }

        public string Account { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string Avatar { get; set; } = null!;

        public string Comment { get; set; } = null!;

        public DateTime CommentTime { get; set; }
    }
    public class CommentSectionViewModel
    {
        public bool IsLogin { get; set; }
        public List<CommentViewModel> Comments { get; set; } = null!;
    }
}
