using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laby
{
    class TResultDataStructure
    {
        private int a;

        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }

        private int b;
    }

    class zadanie2 
    {
        public Boolean Z2;

        public bool Z21 { get => Z2; set => Z2 = value; }
    }

    class Program
    {
        public static Task<TResultDataStructure> OperationTask1()
        {

            return Task.Run(() =>
            {
                return new TResultDataStructure
                {
                    A = 1,
                    B = 2


                };
            });
        }

        public static Task<Boolean> OperationTask2(Boolean Z2)
        {
            return Task.Run(() =>
            {
                Z2 = false;
                return Z2;
            });
        }

        static void Main(string[] args)
        {

            Boolean z2 = true;
            Task task = OperationTask1();
            z2 = OperationTask2(z2).Result;

            Console.WriteLine(z2);
        }
    }
}