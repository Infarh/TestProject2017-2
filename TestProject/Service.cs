using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public static class Service
    {
        public static double Integrate(Func<double, double> f,
            double a, double b, double dx = 0.001)
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

        public static double In_db(this double x)
        {
            return 20 * Math.Log10(x);
        }
    }
}
