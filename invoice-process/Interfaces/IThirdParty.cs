using invoice_process.DTOs;

namespace invoice_process.Interfaces
{
    public interface IThirdParty
    {
        /// <summary>
        ///     Gets the classification from thirdparty.
        /// </summary>
        /// <returns>The classification details.</returns>
        /// <param name="inputDto">expects document and json in request dto</param>
        Task<InvoiceClassificationDto> GetClassification(RequestDto inputDto);

    }
}
