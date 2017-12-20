using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program {
    static void Main(string[] args) {
      Server s = new Server();
      s.Run();

      Client c1 = new Client();
      Client c2 = new Client();
      Client c3 = new Client();
      c1.Connect();
      c2.Connect();
      c3.Connect();

      CancellationTokenSource ct1 = new CancellationTokenSource();
      CancellationTokenSource ct2 = new CancellationTokenSource();
      CancellationTokenSource ct3 = new CancellationTokenSource();

      var client1T = c1.keepPinging("message", ct1.Token);
      var client2T = c2.keepPinging("message", ct2.Token);
      var client3T = c3.keepPinging("message", ct3.Token);
      ct1.CancelAfter(2000);
      ct2.CancelAfter(3000);
      ct3.CancelAfter(4000);

      Task.WaitAll(new Task[] { client1T, client2T, client3T });
      s.StopRunning();

      Console.WriteLine(client1T.Result);
    }
}
