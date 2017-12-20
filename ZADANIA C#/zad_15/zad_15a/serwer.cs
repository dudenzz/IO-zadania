using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Server
    {

        #region zmienne
        int port;
        IPAddress addres;

        TcpListener server;
        bool running;
        Task serverTask;

        #endregion

        #region konstruktory
        public Server(string addres = "127.0.0.1", int port = 2048)
        {

            this.addres = IPAddress.Parse(addres);
            this.port = port;
            server = new TcpListener(this.addres, this.port);

        }

        public Server(IPAddress addres, int port = 2048)
        {

            this.addres = addres;
            this.port = port;
            server = new TcpListener(this.addres, this.port);
        }

        #endregion

        #region metody

        public void Run()
        {

            this.serverTask = RunAsync();
        }


        public async Task RunAsync()
        {
            running = true;
            server.Start();
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                    async (t) =>
                    {
                        int i = t.Result;
                        while (true)
                        {
                            client.GetStream().WriteAsync(buffer, 0, i);
                            i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                        }
                    });
            }
        }

        #endregion


        #region akcesory

        public int Port { get => port; set => port = value; }
        public IPAddress Addres { get => addres; set => addres = value; }
        public bool Running { get => running; }
        public Task ServerTask { get => serverTask; set => serverTask = value; }
        public TcpListener Seerver { get => server; set => server = value; }

        #endregion
    }
}