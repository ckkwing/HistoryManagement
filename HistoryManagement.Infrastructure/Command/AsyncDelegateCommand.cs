using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Infrastructure.Command
{
    public class AsyncDelegateCommand<T> : IAsyncCommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Func<object, Task> _asyncExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncDelegateCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute = null)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute((T)parameter);
        }

        public async void Execute(object parameter)
        {
            await AsyncRunner(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await AsyncRunner(parameter);
        }

        protected virtual async Task AsyncRunner(object parameter)
        {
            await _asyncExecute((T)parameter);
        }
    }
}
