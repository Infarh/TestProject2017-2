using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //Antenna a = new DipoleAntenna();
            //var D = a.GetKND();
            //Console.WriteLine("D0 = {0}", D);

            const int N = 16;
            const double f0 = 3e9;// GHz
            const double c = 3e8; // m/s
            const double wave_len = c / f0;
            const double d = wave_len / 2;

            var elements = new Antenna[N];
            for (var i = 0; i < N; i++)
                elements[i] = new DipoleAntenna();

            var array = new AntennaArray(d / wave_len, elements);

            for(var th = -90.0; th <= 90; th += 2)
                Console.WriteLine("{0: 000;-000}\t|\t{1:0}",
                    th, array.Pattern(th * Math.PI / 180).Magnitude);

            Console.ReadLine();
        }

        public static double Integrate(Func<double, double> f, double a, double b, double dx = 0.001)
        {
            double x = a;
            double S = 0;
            while (x < b)
            {
                var f1 = f(x);
                var f2 = f(x + dx);
                var s = (f1 + f2) / 2 * dx;
                S += s;
                x += dx;
            }
            return S;
        }
    }
}
