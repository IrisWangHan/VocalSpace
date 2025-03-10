using Newtonsoft.Json;

namespace VocalSpace.Models.Test.Selection
{
    public class SelectionListDTO
    {
        //第一步 ：建立DTO
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("selectionCoverPath")]
        public string SelectionCoverPath { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("joinState")]
        public string JoinState { get; set; }

        [JsonProperty("voteState")]
        public string VoteState { get; set; }
        [JsonProperty("selectionId")]
        public long SelectionId { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("votingStartDate")]
        public DateTime VotingStartDate { get; set; }

        [JsonProperty("votingEndDate")]
        public DateTime VotingEndDate { get; set; }
    }


   
    //public class EventDescriptionViewModel
    //{
    //    public SelectionListDTO Selection { get; set; }  // 單筆資料
    //    public IEnumerable<AnotherDTO> AnotherDataList { get; set; }  // 多筆資料
    //}

}
