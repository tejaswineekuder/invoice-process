using invoice_process.DTOs;

namespace invoice_process.Interfaces
{
    public interface IEvaluate
    {
        /// <summary>
        ///    evaluates invoice and summarises.
        /// </summary>
        /// <returns>summary dto</returns>
        /// <param name="">invoice dto with classification info</param>
        Task<EvaluationSummaryDto> InvoiceEvaluation(InvoiceDto invoice);

    }
}
