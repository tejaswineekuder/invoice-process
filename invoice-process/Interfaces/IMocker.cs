namespace invoice_process.Interfaces
{
    public interface IMocker
    {
        /// <summary>
        ///     starts the server.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        Task StartServer();

        /// <summary>
        ///     stops the server.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        Task StopServer();

        /// <summary>
        ///      This defines a mock API response that responds to an incoming HTTP GET call.
        /// </summary>
        /// <returns></returns>
        /// <param name=""></param>
        Task CreateThirdPartyStub();
    }
}
