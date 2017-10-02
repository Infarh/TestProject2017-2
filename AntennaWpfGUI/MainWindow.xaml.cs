using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestProject.Test;

namespace AntennaWpfGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestProject.Test.Vibrator Vibrator = new Vibrator();

        public MainWindow() { InitializeComponent(); }


        private void MenuItem_OnClick(object Sender, RoutedEventArgs E)
        {
            Close();
        }

        private void Length_TextChanged(object sender, TextChangedEventArgs e)
        {
            var L_str = Length.Text;
            if (L_str == null || L_str.Length == 0) return;
            var L = Convert.ToDouble(L_str);
            if(L <= 0 || L >= 0.7) return;
            Vibrator.Length = L;
            var D0 = Vibrator.GetKND();
            Title = D0.ToString("F3");

        }
    }
}
