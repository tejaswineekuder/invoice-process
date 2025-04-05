using Newtonsoft.Json;

namespace invoice_process.DTOs
{
    public class RuleDto
    {
        [JsonProperty("ruleId")]
        public int? RuleId { get; set; }

        [JsonProperty("condition")]
        public string? Condition { get; set; }

        [JsonProperty("action")]
        public string? Action { get; set; }
    }

    public class RuleListDto
    {
        public List<RuleDto> Rules { get; set; } = new List<RuleDto>();
    }
}
