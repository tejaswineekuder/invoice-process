using invoice_process.Helper;
using invoice_process.Interfaces;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace invoice_process.Services
{
    public class MockerService : IMocker
    {
       
        private WireMockServer server;

        /// <summary>
        /// This defines a mock API response that responds to an incoming HTTP GET
        ///   to the `/invoice` endpoint with a response with HTTP status code 200,
        ///a Content-Type header with value `application/json` and a response body
        ///  containing json with rsik level and classification.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        public async Task CreateThirdPartyStub()
        {

            server.Given(
                      Request.Create()
                      .WithPath("/invoice")
                      .UsingPost()
                      .WithHeader("Content-Type", new RegexMatcher("multipart/form-data.*")) // Accepts multipart/form-data with any boundary
                         )
                    .RespondWith(
                     Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(MockResponse.CreateMockResponse()) );
        }

        /// <summary>
        ///     This starts a new mock server instance listening at port 9090.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        public async Task StartServer()
        {
            server = WireMockServer.Start(new WireMock.Settings.WireMockServerSettings
            {
                Urls = ["http://localhost:9090"],
                StartAdminInterface = true,
                ReadStaticMappings = false
            });
        }

        /// <summary>
        ///    This stops the mock server - to clean up after ourselves.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        public async Task StopServer()
        {
            server.Stop();
        }
    }
}