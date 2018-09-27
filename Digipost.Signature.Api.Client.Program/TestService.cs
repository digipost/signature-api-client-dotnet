using System;

namespace Digipost.Signature.Api.Client.Program
{
    public interface ITestService
    {
        void Run();
    }

    public class TestService : ITestService
    {
        public void Run()
        {
            Console.WriteLine("Did run test service");
        }
    }
}
