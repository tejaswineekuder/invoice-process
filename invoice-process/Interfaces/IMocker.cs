namespace invoice_process.Interfaces
{
    public interface IMocker
    {
        Task StartServer();
        Task StopServer();
        Task CreateThirdPartyStub();
    }
}
