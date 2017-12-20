using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laby {
    class TResultDataStructure 
    {
        private int a;

        public int A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public int B
        {
            get
            {
                return b;
            }

            set
            {
                b = value;
            }
        }

        private int b;
    }

    class zadanie2 {

        public Boolean Z2;

        class Program {

            public static Task<TResultDataStructure> OperationTask1() {
                return Task.Run(() => {
                    return new TResultDataStructure {
                        A = 1,
                        B = 2
                    };
                });
            }

            public static Task<Boolean> OperationTask2(Boolean Z2) {
                return Task.Run(() => {
                    Z2 = false;
                    return Z2;
                });
            }


            public static async Task OperationTask3() {
                byte[] data = null;
                WebClient client = new WebClient();
                client.DownloadDataCompleted +=
                   delegate (object sender, DownloadDataCompletedEventArgs e) {
                       data = e.Result;
                   };
                Console.WriteLine("starting...");
                client.DownloadDataAsync(new Uri("http://stackoverflow.com/questions/"));
                while (client.IsBusy) {
                    Console.WriteLine("\twaiting...");
                    Thread.Sleep(100);
                }

                Console.WriteLine("done. {0} bytes received;", data.Length);
            }

            static void Main(string[] args) {
                Boolean z2 = true;
                Task task = OperationTask1();
                z2 = OperationTask2(z2).Result;

                Console.WriteLine(z2);
                OperationTask3();
            }
        }
    } 
}