using Newtonsoft.Json;

namespace invoice_process.DTOs
{
    public class InvoiceDto 
    {
        [JsonProperty("invoiceId")]
        public string? InvoiceId { get; set; }

        [JsonProperty("invoiceNumber")]
        public string? InvoiceNumber { get; set; }

        [JsonProperty("invoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [JsonProperty("comment")]
        public string? Comment { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
    }
}
