using invoice_process.Interfaces;

namespace invoice_process.Services
{
    public class MockerService : IMocker
    {
        public Task CreateThirdPartyStub()
        {
            throw new NotImplementedException();
        }

        public Task StartServer()
        {
            throw new NotImplementedException();
        }

        public Task StopServer()
        {
            throw new NotImplementedException();
        }
    }
}