using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TestProject;

// ReSharper disable UnusedMember.Global

namespace AntennaWpfGUI
{
    public class MainModel : INotifyPropertyChanged
    {
        /// <summary>Шаг между элементами</summary>
        private double f_d = 0.5;
        /// <summary>Число антенных элементов</summary>
        private int f_N = 16;
        /// <summary>Угол отклонения луча</summary>
        private double f_Th0;

        public AntennaArray Antenna { get; private set; }

        /// <summary>Число элементов</summary>
        public int N
        {
            get => f_N;
            set
            {
                if (value == f_N) return;
                f_N = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("N"));
                Antenna = new AntennaArray(f_d, GetAntennas(f_N));
                Antenna.Th0 = Th0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Antenna"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KND"));
                CalculatePattern();
            }
        }

        /// <summary>Шаг между элементами</summary>
        public double d
        {
            get => f_d;
            set
            {
                if (value == f_d) return;
                f_d = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("d"));
                Antenna = new AntennaArray(f_d, GetAntennas(f_N));
                Antenna.Th0 = Th0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Antenna"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KND"));
                CalculatePattern();
            }
        }

        /// <summary>Коэффициент направленного действия</summary>
        public double KND => Antenna?.GetKND().In_db() ?? double.NaN;

        /// <summary>Диаграмма направленности</summary>
        public PatternValue[] Pattern { get; set; }

        /// <summary>Угол отклонения луча</summary>
        public double Th0
        {
            get => f_Th0;
            set
            {
                if (f_Th0 == value) return;
                f_Th0 = value;
                Antenna.Th0 = value * Math.PI / 180;
                CalculatePattern();
            }
        }

        public double LeftBeamEdge07 { get; private set; }
        public double RightBeamEdge07 { get; private set; }
        public double BeamWidth07 => RightBeamEdge07 - LeftBeamEdge07;
        public double UBL { get; private set; }

        public MainModel()
        {
            Antenna = new AntennaArray(f_d, GetAntennas(f_N));
            CalculatePattern();
        }

        /// <summary>Расчёт значений ДН</summary>
        private void CalculatePattern()
        {
            const double th_start = -90;
            const double th_end = 90;
            const double dth = 0.1;

            const double delta_th = th_end - th_start;
            const int N = (int)(delta_th / dth) + 1;

            var data = new PatternValue[N];

            const double toRad = Math.PI / 180;
            for (var i = 0; i < N; i++)
                data[i] = new PatternValue
                {
                    Angle = i * dth + th_start,
                    Value = Antenna.Pattern((i * dth + th_start) * toRad).Magnitude
                };
            Pattern = data;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pattern"));
            AnalysePatter(data);
        }

        private void AnalysePatter(PatternValue[] pattern)
        {
            var max = pattern[0].Value;
            var max_index = 0;
            for (var i = 0; i < pattern.Length; i++)
            {
                var value = pattern[i].Value;
                if (value > max)
                {
                    max = value;
                    max_index = i;
                }
            }
            var max_angle = pattern[max_index].Angle;

            var max_sqrt2 = max / Math.Sqrt(2);
            var left_beam_edge07 = max_index;
            for (var i = max_index; i > 0; i--)
            {
                var value = pattern[i].Value;
                if (value < max_sqrt2)
                {
                    left_beam_edge07 = i;
                    break;
                }
            }
            LeftBeamEdge07 = pattern[left_beam_edge07].Angle;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LeftBeamEdge07"));
            var right_beam_edge07 = max_index;
            for (var i = max_index; i < pattern.Length; i++)
            {
                var value = pattern[i].Value;
                if (value < max_sqrt2)
                {
                    right_beam_edge07 = i;
                    break;
                }
            }
            RightBeamEdge07 = pattern[right_beam_edge07].Angle;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RightBeamEdge07"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BeamWidth07"));

            var left_beam_edge0 = pattern[left_beam_edge07].Angle * 2;
            var right_beam_edge0 = pattern[right_beam_edge07].Angle * 2;

            var ubl = 0d;
            for (var i = 0; i < pattern.Length; i++)
                if (pattern[i].Angle < left_beam_edge0 || pattern[i].Angle > right_beam_edge0)
                {
                    var value = pattern[i].Value;
                    if (ubl < value)
                        ubl = value;
                }
            UBL = 20*Math.Log10(ubl/max);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UBL"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static Antenna[] GetAntennas(int N)
        {
            var result = new Antenna[N];
            for (var i = 0; i < N; i++) result[i] = new DipoleAntenna();
            return result;
        }
    }

    /// <summary>Значение диаграммы направленности</summary>
    public class PatternValue
    {
        /// <summary>Угол</summary>
        public double Angle { get; set; }
        /// <summary>Амплитудное значение ДН</summary>
        public double Value { get; set; }
        /// <summary>Значение ДН в дБ</summary>
        public double Value_in_db => Value.In_db();
    }
}