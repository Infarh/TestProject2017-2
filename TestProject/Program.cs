using System;
using System.IO;

namespace TestProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Test.Vibrator antenna;
            antenna = new Test.Vibrator();
            for (double L = 0.1; L < 0.7; L += 0.1)
            {
                antenna.Length = L;
                var D0 = antenna.GetKND();
                Console.Write("L = " + L.ToString());
                Console.Write(" D0 = " + D0.ToString("f3"));
                Console.WriteLine(" D0[db] = " + D0.In_db());
            }

            Console.ReadLine();
        }
    }
}
