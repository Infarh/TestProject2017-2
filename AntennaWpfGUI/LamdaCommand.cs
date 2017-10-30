using System;
using System.Windows.Input;

namespace AntennaWpfGUI
{
    public class LamdaCommand : ICommand
    {
        private readonly Action f_Action;

        public LamdaCommand(Action execute)
        {
            f_Action = execute;
        }

        public event EventHandler CanExecuteChanged { add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            f_Action();
        }
    }
}