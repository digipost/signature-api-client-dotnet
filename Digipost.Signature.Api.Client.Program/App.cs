namespace Digipost.Signature.Api.Client.Program
{
    public class App
    {
        private readonly ITestService _testService;

        public App(ITestService testService)
        {
            _testService = testService;
        }

        public void Run()
        {
            _testService.Run();
            System.Console.ReadKey();
        }
    }
}
