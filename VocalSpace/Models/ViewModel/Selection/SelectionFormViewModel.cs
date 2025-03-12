using Newtonsoft.Json;

namespace VocalSpace.Models.ViewModel.Selection
{
    public class SelectionFormViewModel
    {
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }


        [JsonProperty("file")]
        public IFormFile? File { get; set; }

    }
}
