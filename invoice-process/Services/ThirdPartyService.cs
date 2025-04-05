using invoice_process.DTOs;
using invoice_process.Interfaces;

namespace invoice_process.Services
{
    public class ThirdPartyService : IThirdParty
    {
        public Task<InvoiceClassificationDto> GetClassification(RequestDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
