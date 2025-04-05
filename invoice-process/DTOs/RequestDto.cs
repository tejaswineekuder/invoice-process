using System.ComponentModel.DataAnnotations;

namespace invoice_process.DTOs
{
    public class RequestDto
    {
        [Required]
        public IFormFile? PdfFile { get; set; }

        [Required]
        public string? JsonPayload { get; set; }
    }
}
