namespace VocalSpace.Models.ViewModel.Song
{
    public class SongUploadModel
    {
        public required string songName { get; set; }
        public required int aritistId { get; set; }
        public required IFormFile coverImage { get; set; }
        public required IFormFile audioFile { get; set; }
        public required byte categoryID { get; set; }
        public string? description { get; set; }
        public string? lyrics { get; set; }
        public required DateTime createTime { get; set; }
    }
}
