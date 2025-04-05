using invoice_process.DTOs;
using invoice_process.Helper;
using invoice_process.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace invoice_process.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvaluateController : ControllerBase
    {
        private readonly ILogger<EvaluateController> _logger;
        private readonly IThirdParty _thirdParty;
        private readonly IDecisionMaker _decisionMaker;
        private readonly IEvaluate _evaluate;

        public EvaluateController(ILogger<EvaluateController> logger, IThirdParty thirdParty, IDecisionMaker decisionMaker, IEvaluate evaluate)
        {
            _logger = logger;
            _thirdParty = thirdParty;
            _decisionMaker = decisionMaker;
            _evaluate = evaluate;
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

            //deserialize the json payload to the InvoiceDto object
            var invoice = JsonConvert.DeserializeObject<InvoiceDto>(request.JsonPayload);
            if (invoice == null)
            {
                _logger.LogError("Failed to deserialize JSON payload: {JsonPayload}", request.JsonPayload);
                return BadRequest("Invalid JSON payload.");
            }
            if (invoice.InvoiceId == null)
            {
                _logger.LogError("Failed to deserialize JSON payload: {JsonPayload}", request.JsonPayload);
                return BadRequest("Invalid JSON payload.");
            }

            //get classification information from third party
            var classification = await _thirdParty.GetClassification(request);
            if (classification == null)
            {
                return BadRequest("Failed to get classification from third party.");
            }
            _logger.LogInformation("Invoice classification is obtained, moving on to decision rule processing");

            //merge the classification information with the invoice
            invoice.Classification = classification.Classification;
            invoice.RiskLevel = classification.RiskLevel;

            //get the action from the rules
            var action = _decisionMaker.GetAction(invoice);
            _logger.LogInformation("Decision rule processing action : " + action);

            //get the evaluation summary
            var evaluationSummary = await _evaluate.InvoiceEvaluation(invoice);
            if (evaluationSummary == null)
            {
                return BadRequest("Failed to get evaluation summary.");
            }
            evaluationSummary.RulesApplied = action;
            //write evaluation summary to a text document 
            // Convert text to byte array
            byte[] fileBytes = Encoding.UTF8.GetBytes(evaluationSummary.EvaluationSummary);

            // Convert file contents to Base64 string
            evaluationSummary.EvaluationTextFile = Convert.ToBase64String(fileBytes);

            _logger.LogInformation("Invoice evaluation summary : " + JsonConvert.SerializeObject(evaluationSummary));


            // Return both attachment & summary
            return Ok(new
            {
                EvaluationSummary = evaluationSummary,
                //FileDownload = File(fileBytes, "application/octet-stream", "summary.txt")
            });
        }
    }
}
