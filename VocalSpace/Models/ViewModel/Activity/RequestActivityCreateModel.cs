namespace VocalSpace.Models.ViewModel.Activity
{
    public class RequestActivityCreateModel
    {
        public required string Title { get; set; } = null!;

        public required DateTime EventDate { get; set; }

        public required string Location { get; set; } = null!;

        public required string City { get; set; } = null!;

        public string? ActivityDescription { get; set; }

        public required IFormFile CoverImage { get; set; }
    }
}
