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

        static void Main(string[] args)
        {
            string path = "text.txt";

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];

                IAsyncResult result = fs.BeginRead(b, 0, b.Length, null, null);
                fs.EndRead(result);

                UTF8Encoding temp = new UTF8Encoding(true);
                Console.WriteLine(temp.GetString(b));

                fs.Close();
            }
        }

    }
}