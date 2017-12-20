using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static Object thisLock = new Object();

        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ServerThreadProc);

            ThreadPool.QueueUserWorkItem(ClientThreadProc, "1st");
            ThreadPool.QueueUserWorkItem(ClientThreadProc, "2nd");

            while(true) { }
        }

        static void ServerThreadProc(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();

            Console.WriteLine("Nasluchiwanie servera");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(NewClientThreadProc, client);
            }
        }

        static void NewClientThreadProc(Object stateInfo)
        {
            Console.WriteLine("[Server] Nawiazano polaczenie z serwerem");

            TcpClient client = (TcpClient) stateInfo;
            byte[] buffer = new byte[1024];

            while(true)
            {
                int bytes = client.GetStream().Read(buffer, 0, 1024);

                if (bytes == 0)
                    break;

                string msg = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                writeConsoleMessage("[Server] Otrzymano wiadomosc: " + msg, ConsoleColor.Red);

                client.GetStream().Write(buffer, 0, bytes);
            }

            client.Close();
        }

        static void ClientThreadProc(Object stateInfo)
        {
            Console.WriteLine("[Klient] Proba nawiazania polaczenia");
            TcpClient client = new TcpClient("localhost", 2048);
            Console.WriteLine("[Klient] Nawiazano polczenie");

            byte[] buffer = new byte[1024];
            buffer = System.Text.Encoding.ASCII.GetBytes( (string) stateInfo );

            client.GetStream().Write(buffer, 0, buffer.Length);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                buffer = new byte[1024];
                int bytes = client.GetStream().Read(buffer, 0, 1024);

                if (bytes == 0 || watch.Elapsed.Seconds > 5)
                    break;

                string msg = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                writeConsoleMessage("[Klient] Otrzymano wiadomosc: " + msg, ConsoleColor.Green);
                
                client.GetStream().Write(buffer, 0, bytes);
            }

            Console.WriteLine("[Klient] Zakonczono polczenie");
        }

        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock (thisLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

    }
}
