using invoice_process.DTOs;
using RestSharp;

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

        /// <summary>
        ///     Gets the response from restsharp with retry policy.
        /// </summary>
        /// <returns>The third party response with classification details</returns>
        /// <param name="inputDto">expects restclient and request</param>
        Task<RestResponse<InvoiceClassificationDto>> RestResponseWithPolicy(IRestClient restClient, RestRequest restRequest);

    }
}
