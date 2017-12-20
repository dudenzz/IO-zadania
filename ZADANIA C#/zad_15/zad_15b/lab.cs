using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_Lab4 {
    class Server {
        #region Variables
        bool running;
        TcpListener listener;
        Task serverTask;
        #endregion
        #region Properties
        public IPAddress Address {
            get { return Address; }
            set {
                if (!running) {
                    Address = value; listener = new TcpListener(Address, Port);
                } else throw new Exception("ktos probuje zmienic port, gdy serwer jest uruchomiony");
            }
        }
        public int Port {
            get { return Port; }
            set {
                if (!running) {
                    Port = value; listener = new TcpListener(Address, Port);
                } else throw new Exception("ktos probuje zmienic port, gdy serwer jest uruchomiony");
            }
        }

        static readonly int BufforSize = 2048;
        public bool Running { get => running; }
        public Task ServerTask { get => serverTask; }
        #endregion
        #region Constructors
        public Server(string address = "127.0.0.1", int port = 1024) {
            Address = IPAddress.Parse(address);
            Port = port;
            listener = new TcpListener(Address, Port);
        }
        public Server(IPAddress address, int port = 1024) {
            Address = address;
            Port = port;
            listener = new TcpListener(Address, Port);
        }
        #endregion
        #region Methods
       

        public void Start() {
            serverTask = runAsync();
        }
        async Task runAsync() {
            listener.Start();
            while (true) {
                TcpClient client = await listener.AcceptTcpClientAsync();
                byte[] buffer = new byte[BufforSize];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                async (t) => {
                    int i = t.Result;
                    while (true) {
                        await client.GetStream().WriteAsync(buffer, 0, i);
                        i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    }
                });
            }

        }
        #endregion
    }
    class Client {
        #region variables
        TcpClient client;
        #endregion
        #region properties
        #endregion
        #region Constructors
        #endregion
        #region Methods
        public void Connect() {
            client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
        }
        public async Task<string> Ping(string message) {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            buffer = new byte[1024];
            var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, t);
        }
        public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token) {
            List<string> messages = new List<string>();
            bool done = false;
            while (!done) {
                if (token.IsCancellationRequested)
                    done = true;
                messages.Add(await Ping(message));
            }
            return messages;
        }
        #endregion
    }

   
        }
        #endregion
        #region Constructors
        public Server2() {
            Address = IPAddress.Any;
            port = 2048;
        }
        public Server2(int port) {
            this.port = port;
        }
        public Server2(IPAddress address) {
            this.address = address;
        }
        #endregion
        #region Methods

        public async Task RunAsync(CancellationToken ct) {

            server = new TcpListener(address, port);

            try {
                server.Start();
                running = true;
            } catch (SocketException ex) {
                throw (ex);
            }
            while (true && !ct.IsCancellationRequested) {

                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                using (ct.Register(() => client.GetStream().Close())) {
                   await client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct).ContinueWith(
                        async (t) => {
                            int i = t.Result;
                            while (true) {
                                Console.WriteLine(new UTF8Encoding().GetString(buffer,0,i));
                                client.GetStream().WriteAsync(buffer, 0, i, ct);
                                try {
                                    i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct);
                                } catch {
                                    break;
                                }
                            }
                        });
                }
            }

        }
        public void RequestCancellation() {
            cts.Cancel();
            //serverTask.Wait();
            //serverTask.Dispose();
            server.Stop();
        }
        public void Run() {

            serverTask = RunAsync(cts.Token);
        }
        public void StopRunning() {
            RequestCancellation();
            //serverTask.Dispose();
        }
        #endregion
    }


    class Program {

        static public void Zadanie48() {
            Server2 s = new Server2();
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
           // s.StopRunning();
        }

        static async Task<string> ClientTask() {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            await client.GetStream().WriteAsync(message, 0, message.Length);
            return message.ToString();
        }

        static void Main(string[] args) {

            Zadanie48();
            Console.ReadLine();
        }
    }
}

