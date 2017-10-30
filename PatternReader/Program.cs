using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternReader
{
    class Program
    {
        private const string FileName = "DipolePattern.txt";

        static void Main(string[] args)
        {
            var file_info = new FileInfo(FileName);

            var reader = file_info.OpenText();
            reader.ReadLine();
            reader.ReadLine();

            var data = new List<PatternValue>();

            var old_angle = -1d;
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                str = str.Replace('.', ',');

                var items = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var angle_str = items[0];
                var f_str = items[2];

                var angle = double.Parse(angle_str);
                var f = double.Parse(f_str);

                if (old_angle > angle) break;
                old_angle = angle;

                data.Add(new PatternValue { Angle = angle, Value = f });
            }
            reader.Close();
            Console.WriteLine("Файл закончился.");

            Console.ReadLine();
        }
    }

    /// <summary>Отсчёт диаграммы направленности</summary>
    public struct PatternValue
    {
        /// <summary>Угол измерения диаграммы ынаправленности</summary>
        public double Angle;

        public double Ang { get { return Angle; } }

        /// <summary>Значения ДН</summary>
        public double Value;

        public double Val { get { return Value; } }

        public double Val_db { get { return GetValue_In_db(); } }

        public PatternValue(double Angle, double Value)
        {
            this.Angle = Angle;
            this.Value = Value;
        }

        public double GetValue_In_db()
        {
            return 20 * Math.Log10(Value);
        }
    }
}
