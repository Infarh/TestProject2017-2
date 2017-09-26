using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestProject;

namespace AntennaWpfGUI
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string f_Title;
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

        private AntennaArray f_AntennaArray;
        private int f_N = 16;
        private double f_d = 0.5;

        public AntennaArray Antenna => f_AntennaArray;

        private static Antenna[] GetAntennas(int N)
        {
            var result = new Antenna[N];
            for(var i = 0; i < N; i++)
                result[i] =new DipoleAntenna();
            return result;
        }

        public int N
        {
            get => f_N;
            set
            {
                f_N = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("N"));
                f_AntennaArray = new AntennaArray(f_d, GetAntennas(f_N));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Antenna"));
            }
        }

        public double d
        {
            get => f_d;
            set
            {
                f_d = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("d"));
                f_AntennaArray = new AntennaArray(f_d, GetAntennas(f_N));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Antenna"));
            }
        }

        public MainModel()
        {
            Title = "Текст главного окна";
            ChangeTitleCommand = new LamdaCommand(() => Title = "Hello World!");
        }
    }

    public class LamdaCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action f_Action;

        public LamdaCommand(Action execute)
        {
            f_Action = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => f_Action();
    }
}
