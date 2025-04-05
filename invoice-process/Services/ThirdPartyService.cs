using invoice_process.DTOs;
using invoice_process.Interfaces;
using Microsoft.OpenApi.Models;
using Polly;
using RestSharp;

namespace invoice_process.Services
{
    public class ThirdPartyService : IThirdParty
    {
        private readonly IMocker _mocker;
        private readonly ILogger<ThirdPartyService> _logger;

        public ThirdPartyService(IMocker thirdPartyMocker, ILogger<ThirdPartyService> logger)
        {
            _mocker = thirdPartyMocker;
            _logger = logger;
        }

        /// <summary>
        /// This mocks a thirdparty server call and uses restshrp to do so. 
        /// wiremock server is initiated and an endpoint invoice with a stub is created
        /// the endoint expects a file and a json payload
        /// the stub will return a json response with the classification information
        /// </summary>
        /// <returns>classification information</returns>
        /// <param name="">request dto</param>
        public async Task<InvoiceClassificationDto> GetClassification(RequestDto inputDto)
        {
            _mocker.StartServer();
            _mocker.CreateThirdPartyStub();

            // create a client using the options object

            var options = new RestClientOptions("http://localhost:9090")
            {
                MaxTimeout = 1000,
                ThrowOnAnyError = true
            };
            var client = new RestClient(options);
            var request = new RestRequest("/invoice", Method.Post);

            // Converting the IFormFile to a stream and add it to the request
            using (var stream = new MemoryStream())
            {
                await inputDto?.PdfFile?.CopyToAsync(stream);  // copy the IFormFile to a MemoryStream
                stream.Position = 0;  // reset stream position to the beginning

                // Add the file directly from the stream
                request.AddFile("file", stream.ToArray(), "test.pdf", "application/pdf");
            }
            // Adding the JSON payload as a parameter
            request.AddParameter("jsonPayload", inputDto.JsonPayload, ParameterType.GetOrPost);

            // rest client with retry policy
            var invoiceClassification = await RestResponseWithPolicy(client, request);
            if (!invoiceClassification.IsSuccessful)
            {
                _logger.LogError($"The request failed. HttpStatusCode={invoiceClassification.StatusCode}. Uri={invoiceClassification.ResponseUri}; RequestResponse={invoiceClassification.Content}");
                return null;
            }

            _mocker.StopServer();

            return invoiceClassification.Data;
        }

        /// <summary>
        /// Here we use Polly to implement retrying thrid party call, in this case 3 times. 
        /// </summary>
        /// <returns>classification information</returns>
        /// <param name="">restsharp client and request</param>
        public async Task<RestResponse<InvoiceClassificationDto>> RestResponseWithPolicy(IRestClient restClient, RestRequest restRequest)
        {
            //setting retry mechanism
            int maxRetryAttempts = 3;
            TimeSpan delayBetweenFailures = TimeSpan.FromSeconds(2);

            // create a retry policy that retries on failure
            var retryPolicy = Policy
                             .HandleResult<RestResponse<InvoiceClassificationDto>>(x => !x.IsSuccessful)
                             .WaitAndRetryAsync(maxRetryAttempts, x => delayBetweenFailures, (iRestResponse, timeSpan, retryCount, context) =>
                             {
                                 _logger.LogWarning($"The request failed. HttpStatusCode={iRestResponse.Result.StatusCode}. Waiting {timeSpan} seconds before retry. Number attempt {retryCount}. Uri={iRestResponse.Result.ResponseUri}; RequestResponse={iRestResponse.Result.Content}");
                             });

            var invoiceClassification = await retryPolicy.ExecuteAsync(
                                      () => restClient.ExecuteAsync<InvoiceClassificationDto>(restRequest));

            return invoiceClassification;
        }
    }
}
