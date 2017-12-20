using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mo
{
    class Program
    {
        delegate int DelegateType(object arguments); // typ funkcji co chcemy ja przechowywac
        static DelegateType delegateRecursiveFib;
        static DelegateType delegateIterativeFib;

        static int rFib(object arguments)
        {
            int i = (int) arguments;

            if (i == 1 || i == 2)
                return 1;

            return rFib(i-1) + rFib(i-2);
        }

        static int iFib(object arguments)
        {
            int j = (int)arguments;

            if (j == 1 || j == 2)
                return 1;

            int result = 0;
            int k1 = 1;
            int k2 = 1;

            for (int i = 2; i < j; ++i)
            {
                result = k1 + k2;
                k2 = k1;
                k1 = result;
            }

            return result;
        }

        static void Main(string[] args)
        {
            int i = 40;

            delegateRecursiveFib = new DelegateType(rFib);
            delegateIterativeFib = new DelegateType(iFib);

            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            IAsyncResult r_rFib = delegateRecursiveFib.BeginInvoke(i, null, null);
            int r1 = delegateRecursiveFib.EndInvoke(r_rFib);
            stopWatch1.Stop();

            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            IAsyncResult r_iFib = delegateIterativeFib.BeginInvoke(i, null, null);
            int r2 = delegateIterativeFib.EndInvoke(r_iFib);
            stopWatch2.Stop();

            Console.WriteLine("Recursive: " + r1 + " " + stopWatch1.ElapsedMilliseconds);
            Console.WriteLine("Iterative: " + r2 + " " + stopWatch2.ElapsedMilliseconds);
        }

    }
}