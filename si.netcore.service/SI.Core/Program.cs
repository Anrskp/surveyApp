using Newtonsoft.Json;
using SI.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemIntegration_2018;
using SystemIntegration_2018.Models;

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
