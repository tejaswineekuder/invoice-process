using invoice_process.Controllers;
using invoice_process.DTOs;
using invoice_process.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_process.Tests.Controllers
{
    public class EvaluateControllerTests
    {
        private readonly ILogger<EvaluateController> _logger;
        private readonly IEvaluate _evaluate;
        private readonly IThirdParty _thirdParty;
        private readonly IDecisionMaker _decisionMaker;
        private EvaluateController _evaluateController;
        public EvaluateControllerTests()
        {
            _thirdParty = Substitute.For<IThirdParty>(); 
            _decisionMaker = Substitute.For<IDecisionMaker>(); 
            _evaluate = Substitute.For<IEvaluate>(); 
            _logger = Substitute.For<ILogger<EvaluateController>>(); 
        }

        [Test]
        public async Task Evaluate_ShouldReturnBadRequest_WhenPdfFileIsNull()
        {
            // Arrange
            var request = new RequestDto
            {
                PdfFile = null,
                JsonPayload = "{\"invoiceId\": \"12345\", \"invoiceNumber\" : \"S12345\", " +
                "               \"invoiceDate\": \"2025-04-04\", \"comment\": \"by insurance company\", \"amount\": 100.00}"
            };

            _evaluateController = new EvaluateController(_logger,_thirdParty,_decisionMaker,_evaluate);

            // Act
            var result = await _evaluateController.Evaluate(request);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("PDF file is required.", badRequestResult.Value);

        }

        [Test]
        public async Task Evaluate_ShouldReturnOk_WhenAllValid()
        {
            // Arrange

            var pdfFile = Substitute.For<IFormFile>();
            pdfFile.Length.Returns(1024); // Simulate a non-empty file
            var request = new RequestDto
            {
                PdfFile = pdfFile,
                JsonPayload = "{\"invoiceId\": \"12345\", \"invoiceNumber\" : \"S12345\", " +
                "               \"invoiceDate\": \"2025-04-04\", \"comment\": \"by insurance company\", \"amount\": 1000.00}"
            };
            var invoice = new InvoiceDto
            {
                InvoiceId = "12345",
                InvoiceNumber = "S12345",
                InvoiceDate = DateTime.Now,
                Comment = "by insurance company",
                Amount = 1000
            };

            var classification = new InvoiceClassificationDto
            {
                Classification = "Approved",
                RiskLevel = "Low"
            };

            var evaluationSummary = new EvaluationSummaryDto
            {
                EvaluationId = Guid.NewGuid().ToString(),
                InvoiceId = "12345",
                RulesApplied = "Approved",
                Classification = "Approved"
            };

            _thirdParty.GetClassification(request).Returns(classification);
            _decisionMaker.GetAction(invoice).Returns("Approve");
            _evaluate.InvoiceEvaluation(Arg.Any<InvoiceDto>()).Returns(evaluationSummary);

            _evaluateController = new EvaluateController(_logger, _thirdParty,_decisionMaker,_evaluate);

            // Act
            var result = await _evaluateController.Evaluate(request);

            // Assert

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
