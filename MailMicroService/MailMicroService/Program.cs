using System;

namespace MailMicroService
{
    class Program
    {
        static void Main(string[] args)
        {
            Receiver receiver = new Receiver();
            receiver.StartRPC();
        }
    }
}
