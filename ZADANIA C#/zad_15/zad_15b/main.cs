using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2 {
    class Program {
        static void Main(string[] args) {
            Server s = new Server();
            Client k1 = new Client();
            Client k2 = new Client();
            s.Run();

            k1.Connect();
            k2.Connect();
            var client1T = k1.Ping("1");
            var client2T = k2.Ping("2");

            Task.WaitAll(new Task[] { client1T, client2T });

            Console.WriteLine(client1T.Result);
            Console.WriteLine(client2T.Result);
        }
    }
}