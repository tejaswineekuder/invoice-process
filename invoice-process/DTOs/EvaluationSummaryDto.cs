using Newtonsoft.Json;

namespace invoice_process.DTOs
{
    public class EvaluationSummaryDto
    {
        [JsonProperty("evaluationId")]
        public string? EvaluationId { get; set; }

        [JsonProperty("invoiceId")]
        public string? InvoiceId { get; set; }

        [JsonProperty("rulesApplied")]
        public string? RulesApplied { get; set; }

        [JsonProperty("classification")]
        public string? Classification { get; set; }

        [JsonProperty("evaluationTextFile")]
        public string? EvaluationTextFile { get; set; }
        public string? EvaluationSummary
        {
            get
            {
                return $"Summary of the Invoice : Evaluation ID: {EvaluationId}, Invoice ID: {InvoiceId}, Rules Applied: {RulesApplied}, Classification: {Classification}";
            }
        }
    }
}
