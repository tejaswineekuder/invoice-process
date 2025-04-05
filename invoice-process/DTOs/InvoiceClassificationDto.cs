using Newtonsoft.Json;

namespace invoice_process.DTOs
{
    public class InvoiceClassificationDto
    {

        [JsonProperty("classification")]
        public string? Classification { get; set; }

        [JsonProperty("riskLevel")]
        public string? RiskLevel { get; set; }
    }
}
