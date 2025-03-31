using Newtonsoft.Json;

namespace VocalSpace.Models.ViewModel.Selection
{
    public class SelectionListViewModel
    {
        //第一步 ：建立DTO
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("selectionCoverPath")]
        public string? SelectionCoverPath { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("joinState")]
        public string? JoinState { get; set; }

        [JsonProperty("voteState")]
        public string? VoteState { get; set; }
        [JsonProperty("selectionId")]
        public long SelectionId { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("votingStartDate")]
        public DateTime? VotingStartDate { get; set; }

        [JsonProperty("votingEndDate")]
        public DateTime? VotingEndDate { get; set; }

        [JsonProperty("paginationCount")]
        public int PaginationCount { get; set; }
        public List<SelectionSongs> Songs { get; set; } = new List<SelectionSongs>();

    }
    public class SelectionSongs
    {
        public long OldSelectionDetailId { get; set; }
        public int? OldVoteCount { get; set; }
        public long SelectionDetailId { get; set; }
        public long SongId { get; set; }
        public int? VoteCount { get; set; }
        public string? CoverPhotoPath { get; set; }
        public string? SongDescription { get; set; }
        public string? SongName { get; set; }
        public int? LikeCount { get; set; }
        public string? SongPath { get; set; }
        public bool? IsLiked { get; set; }
        public bool? IsVoted { get; set; }
    }
}
