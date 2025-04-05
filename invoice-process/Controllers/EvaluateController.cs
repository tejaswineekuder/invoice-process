using invoice_process.DTOs;
using invoice_process.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace invoice_process.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvaluateController : ControllerBase
    {
        private readonly ILogger<EvaluateController> _logger;
        private readonly IThirdParty _thirdParty;

        public EvaluateController(ILogger<EvaluateController> logger, IThirdParty thirdParty)
        {
            _logger = logger;
            _thirdParty = thirdParty;
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
            _logger.LogInformation("Input verified , moving further to fetch invoice classification");

            //get classification information from third party
            var classification = await _thirdParty.GetClassification(request);
            if (classification == null)
            {
                return BadRequest("Failed to get classification from third party.");
            }
            _logger.LogInformation("Invoice classification is obtained, moving on to decision table");

            return Ok(new {Classification = classification});
        }
    }
}
