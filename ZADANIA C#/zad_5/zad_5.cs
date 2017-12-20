using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zad5
{
    class Program
    {
        static Object thisLock = new Object();

        static int[] tab;
        static int sum;

        static int size;
        static int fragmentSize;

        static void Main(string[] args)
        {
            size = args.Length > 0 ? int.Parse(args[0]) : 640;
            fragmentSize = args.Length > 1 ? int.Parse(args[1]) : 10;

            tab = new int[size];
            sum = 0;

            //

            Random rand = new Random();
            for(int i = 0; i < size; ++i)
            {
                tab[i] = rand.Next();
            }

            //
            
            int workersCount = (int) Math.Ceiling((double) size / fragmentSize);
            AutoResetEvent[] events = new AutoResetEvent[workersCount];

            for (int i = 0; i < workersCount; ++i)
            {
                events[i] = new AutoResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(Sum), new object[] { events[i], i });
            }


            WaitHandle.WaitAll(events);

            //
            int suma = 0;
            foreach (int i in tab)
            {
                suma += i;
            }

            Console.WriteLine(suma);
            Console.WriteLine(sum);
        }

        static void Sum(object state)
        {
            AutoResetEvent e = (AutoResetEvent)((object[])state)[0];
            int from = (int)((object[])state)[1] * fragmentSize;
            int result = 0;

            for (int i = from; i < from + fragmentSize && i < size; ++i)
            {
                result += tab[i];
            }

            lock (thisLock)
            {
                sum += result;
            }

            e.Set();
        }
    }
}
