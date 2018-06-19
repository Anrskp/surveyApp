using SystemIntegration_2018;

namespace SI.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Receiver receiver = new Receiver();
            receiver.StartRPCListener();
        }
    }
}
