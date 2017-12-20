using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Client
    {
        #region variables
        TcpClient client;
        IPAddress addres;
        int port;


        #endregion
        #region akcesory

        public TcpClient Clieent { get => client; set => client = value; }
        public IPAddress Addres { get => addres; set => addres = value; }
        public int Port { get => port; set => port = value; }


        #endregion
        #region Constructors

        public Client(string addres = "127.0.0.1", int port = 2048)
        {

            this.addres = IPAddress.Parse(addres);
            this.port = port;
          

        }

        public Client(IPAddress addres, int port = 2048)
        {

            this.addres = addres;
            this.port = port;
            
        }

        #endregion
        #region Methods
        public void Connect()
        {
            client = new TcpClient();
            client.Connect(new IPEndPoint(addres, port));
        }
        public async Task<string> Ping(string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            buffer = new byte[1024];
            var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, t);
        }
        public async Task<IEnumerable<string>> KeepPinging(string message)
        {
            List<string> messages = new List<string>();
            bool done = false;
            while (!done)
            {
               
                messages.Add(await Ping(message));
            }
            return messages;
        }
        #endregion
    }
}