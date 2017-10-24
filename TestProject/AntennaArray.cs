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
        /// <summary>Массив антенных элементов</summary>
        private Antenna[] f_Antennas;
        /// <summary>Шаг между излучателями</summary>
        private double f_d;

        /// <summary>Число излучателей</summary>
        public int N => f_Antennas.Length;

        /// <summary>Шаг между излучателями</summary>
        public double d => f_d;

        /// <summary>Размер апертуры</summary>
        public double Length => f_d * (N-1);

        /// <summary>Угол фазирования</summary>
        public double Th0 { get; set; }

        /// <summary>Амплитудное рапределение по апертуре</summary>
        public Func<double, double> A { get; set; } = x => 1;

        public AntennaArray(double d, Antenna[] antennas)
        {
            f_Antennas = antennas;
            f_d = d;
        }

        public override Complex Pattern(double th)
        {
            var F = new Complex(0,0);
            var L = Length;
            for (var i = 0; i < f_Antennas.Length; i++)
            {
                var x = i * f_d - L / 2;
                var f = A(x) * f_Antennas[i].Pattern(th);
                F += f * Complex.Exp(-Complex.ImaginaryOne
                                     * 2 * Math.PI * f_d * i 
                                     * (Math.Sin(th) - Math.Sin(Th0)));
            }
            return F;
        }
    }
}
