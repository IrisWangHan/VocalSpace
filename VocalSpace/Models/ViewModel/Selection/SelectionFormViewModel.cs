using Newtonsoft.Json;

namespace VocalSpace.Models.ViewModel.Selection
{
    public class SelectionFormViewModel : SelectionListViewModel
    {
        [JsonProperty("userId")]
        public long? UserId { get; set; }
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("avatarPath")]
        public string? AvatarPath { get; set; }

        [JsonProperty("applyStatusPath")]
        public string? ApplyStatusPath { get; set; }
        [JsonProperty("file")]
        public IFormFile? File { get; set; }
        [JsonProperty("applyStatusText")]
        public string? ApplyStatusText { get; set; }

        [JsonProperty("applyStatus")]
        public int? ApplyStatus { get; set; }
        public List<SelectionSongs> ApplySongs { get; set; } = new List<SelectionSongs>();

    }
    public class ApplicationRequest
    {
        [JsonProperty("selectionId")]
        public long SelectionId { get; set; }
        [JsonProperty("userName")]
        public required string UserName { get; set; }
        [JsonProperty("email")]
        public required string Email { get; set; }
        [JsonProperty("songs")]
        public required List<Song> Songs { get; set; }
    }

    public class Song
    {
        [JsonProperty("songId")]
        public int SongId { get; set; }
        [JsonProperty("songName")]
        public required string SongName { get; set; }
    }

}
