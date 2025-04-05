using invoice_process.DTOs;

namespace invoice_process.Interfaces
{
    public interface IDecisionMaker
    {

        /// <summary>
        ///     gets the action to be made my processing the conditions.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        string GetAction(InvoiceDto invoice);
    }
}
