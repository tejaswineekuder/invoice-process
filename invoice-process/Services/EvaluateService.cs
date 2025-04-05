using invoice_process.DTOs;
using invoice_process.Interfaces;
using Microsoft.CodeAnalysis.Operations;

namespace invoice_process.Services
{
    public class EvaluateService : IEvaluate
    {
        public async Task<EvaluationSummaryDto> InvoiceEvaluation(InvoiceDto invoice)
        {
            //summarize the invoice, classification and rules into an object
            return new EvaluationSummaryDto
            {
                EvaluationId = Guid.NewGuid().ToString(),
                InvoiceId = invoice?.InvoiceId,
                Classification = invoice?.Classification,
            };
        }
    }
}
