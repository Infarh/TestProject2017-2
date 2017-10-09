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
        private double f_d = 0.5;
        private int f_N = 16;

        private string f_Title;

        public MainModel()
        {
            Title = "Текст главного окна";
            ChangeTitleCommand = new LamdaCommand(() => Title = "Hello World!");
        }

        public string Title
        {
            get => f_Title;
            set
            {
                f_Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public ICommand ChangeTitleCommand { get; }

        public AntennaArray Antenna { get; private set; }

        /// <summary>Число элементов</summary>
        public int N
        {
            get => f_N;
            set
            {
                f_N = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("N"));
                Antenna = new AntennaArray(f_d, GetAntennas(f_N));
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
                f_d = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("d"));
                Antenna = new AntennaArray(f_d, GetAntennas(f_N));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Antenna"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KND"));
                CalculatePattern();
            }
        }

        /// <summary>Коэффициент направленного действия</summary>
        public double KND => Antenna?.GetKND().In_db() ?? double.NaN;

        /// <summary>Диаграмма направленности</summary>
        public PatternValue[] Pattern { get; set; }

        private void CalculatePattern()
        {
            const double th_start = -90;
            const double th_end = 90;
            const double dth = 0.5;

            var delta_th = th_end - th_start;
            var N = (int)(delta_th / dth) + 1;

            var data = new PatternValue[N];

            var toRad = Math.PI / 180;
            for (var i = 0; i < N; i++)
                data[i] = new PatternValue
                {
                    Angle = i * dth + th_start,
                    Value = Antenna.Pattern(toRad * (i * dth + th_start)).Magnitude
                };
            Pattern = data;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pattern"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static Antenna[] GetAntennas(int N)
        {
            var result = new Antenna[N];
            for (var i = 0; i < N; i++) result[i] = new DipoleAntenna();
            return result;
        }
    }

    public class PatternValue
    {
        public double Angle { get; set; }
        public double Value { get; set; }
        public double Value_in_db => Value.In_db();
    }
}