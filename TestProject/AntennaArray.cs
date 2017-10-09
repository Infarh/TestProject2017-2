using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class AntennaArray : Antenna
    {
        private Antenna[] f_Antennas;
        private double f_d;

        public int N
        {
            get { return f_Antennas.Length; }
        }

        public double d
        {
            get { return f_d; }
        }

        public double Length
        {
            get { return f_d * (N-1); }
        }

        public AntennaArray(double d, Antenna[] antennas)
        {
            f_Antennas = antennas;
            f_d = d;
        }

        public override Complex Pattern(double th)
        {
            var F = new Complex(0,0);
            for (var i = 0; i < f_Antennas.Length; i++)
            {
                var f = f_Antennas[i].Pattern(th);
                F += f * Complex.Exp(-Complex.ImaginaryOne
                                     * 2 * Math.PI * f_d * i * Math.Sin(th));
            }
            return F;
        }
    }
}
