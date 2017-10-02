using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Test
{
    public abstract class Antenna
    {
        public double GetKND()
        {
            // D0 = 2/Integral[-pi/2..pi/2]{F^2(th)cos(th)}dth
            Func<double, double> f = th =>
            {
                var F = Pattern(th).Magnitude;
                return F * F * Math.Cos(th);
            };
            var I = Service.Integrate(f, -Math.PI / 2, Math.PI / 2, 0.001);
            return 2 / I;
        }

        public abstract Complex Pattern(double th);
    }

    public class Dipole : Antenna
    {
        /// <inheritdoc />
        public override Complex Pattern(double th)
        {
            return Math.Cos(th);
        }
    }

    public class Uniform : Antenna
    {
        /// <inheritdoc />
        public override Complex Pattern(double th)
        {
            return 1;
        }
    }

    public class Vibrator : Antenna
    {
        private double f_Length;

        public double Length
        {
            get { return f_Length; }
            set
            {
                if (value <= 0) throw new InvalidOperationException("Длина вибратора должна быть больше нуля");
                if(value > 0.7)
                    throw new NotSupportedException();
                f_Length = value;
            }
        }

        private double F(double th)
        {
            var L = 2 * Math.PI * f_Length;
            return (Math.Cos(L * Math.Sin(th)) - Math.Cos(L))
                   / Math.Cos(th);
        }

        /// <inheritdoc />
        public override Complex Pattern(double th)
        {
            return F(th) / F(0);
        }
    }
}
