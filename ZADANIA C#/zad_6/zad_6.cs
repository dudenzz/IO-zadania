using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mo
{
    class Program
    {
        private static AutoResetEvent e = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            string path = "text.txt";

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];

                AsyncCallback callBack = new AsyncCallback(read);
                fs.BeginRead(b, 0, b.Length, callBack, new object[] { fs, b });

                e.WaitOne();
            }
        }

        static void read(IAsyncResult result)
        {

            FileStream fs = (FileStream) ((object[]) result.AsyncState)[0];
            byte[] b = (byte[]) ((object[])result.AsyncState)[1];

            UTF8Encoding temp = new UTF8Encoding(true);
            Console.WriteLine(temp.GetString(b));

            fs.Close();
            e.Set();
        }
    }
}