using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JobHunting.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action _executeMethod;

        public RelayCommand(Action executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 0067 // disable warning about event not being used
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public void Execute(object parameter)
        {
            _executeMethod.Invoke();
        }
    }
}
