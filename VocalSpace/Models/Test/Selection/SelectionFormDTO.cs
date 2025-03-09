using Newtonsoft.Json;

namespace VocalSpace.Models.Test.Selection
{
    public class SelectionFormDTO
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("tel")]
        public string Tel { get; set; }

        [JsonProperty("file")]
        public IFormFile File { get; set; }

    }

}

