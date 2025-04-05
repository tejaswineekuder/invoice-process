using invoice_process.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace invoice_process.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvaluateController : ControllerBase
    {
        private readonly ILogger<EvaluateController> _logger;

        public EvaluateController(ILogger<EvaluateController> logger)
        {
            _logger = logger;
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> Evaluate([FromForm] RequestDto request)
        {
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("No PDF file found, cannot be processed further");
                return BadRequest("PDF file is required.");
            }

            if (string.IsNullOrWhiteSpace(request.JsonPayload))
            {
                _logger.LogError("Empty payload, cannot be processed further");
                return BadRequest("JSON payload is required.");
            }
            _logger.LogInformation("Input verified , moving further");

            return Ok("Input verified!");
        }
    }
}
