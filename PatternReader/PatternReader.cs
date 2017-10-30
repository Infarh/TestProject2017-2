using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternReader
{
    public static class DNReader
    {
        public static List<PatternValue> Read(string file)
        {
            var data = new List<PatternValue>();

            var file_stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var reader = new StreamReader(file_stream);

            reader.ReadLine();
            reader.ReadLine();

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
            file_stream.Close();

            for (var i = 0; i < data.Count; i++)
            {
                if (data[i].Angle > 90)
                    data[i] = new PatternValue(data[i].Angle - 180, data[i].Value);
            }

            data = data.OrderBy(v => v.Angle).ToList();

            return data;
        }
    }
}
