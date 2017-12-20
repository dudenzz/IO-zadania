using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 900 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 700 });

            Thread.Sleep(1000);
        }
        static void ThreadProc(Object stateInfo)
        {
            int sleepTime = (int) ((object[])stateInfo)[0];

            Thread.Sleep(sleepTime);
            Console.WriteLine("Czekalem: " + sleepTime);
        }

    }
}
