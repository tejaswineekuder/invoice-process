using Castle.Core.Logging;
using invoice_process.Controllers;
using invoice_process.DTOs;
using invoice_process.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_process.Tests.Services
{
    public class DecisionMakerServiceTests
    {
        private readonly ILogger<DecisionMakerService> _logger;
        private DecisionMakerService _decisionMaker;

        public DecisionMakerServiceTests()
        {
            _logger = Substitute.For<ILogger<DecisionMakerService>>();
        }

        [Test]
        public void GetAction_WhenNoConditionsExist_ReturnNoActionFound()
        {
            //Arrange 
            var invoice = new InvoiceDto
            {
                InvoiceId = "12345",
                InvoiceNumber = "S12345",
                InvoiceDate = DateTime.Now,
                Comment = "by insurance company",
                Amount = 1000,
                Classification = "NA Classification ",
                RiskLevel = "NA RiskLevel"
            };

            //Act
            _decisionMaker = new DecisionMakerService(_logger);
            var result = _decisionMaker.GetAction(invoice);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, "No Action found");
        }
    }
}
