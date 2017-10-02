using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public abstract class Antenna
    {
        public abstract Complex Pattern(double th);

        public double GetKND()
        {
            Func<double, double> F = th =>
            {
                var f = Pattern(th).Magnitude;
                return f * f * Math.Cos(th);
            };

            var I = Service.Integrate(F, -Math.PI/2, Math.PI/2);
            return 2 / I;
        }
    }

    public class UniformAntenna : Antenna
    {
        public override Complex Pattern(double th)
        {
            return 1;
        }
    }

    public class DipoleAntenna : Antenna
    {
        public override Complex Pattern(double th)
        {
            return Math.Cos(th);
        }
    }

    public class VibratorAntenna  : Antenna
    {
        public override Complex Pattern(double th)
        {
            throw new NotImplementedException();
        }
    }
}
