using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Infrastructure.Command
{
    public class AsyncCompositeCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private IList<IAsyncCommand> commands = new List<IAsyncCommand>();
        public IEnumerable<IAsyncCommand> RegisteredCommands
        {
            get
            {
                return commands;
            }
        }

        private Action<object> executedCallback;

        public AsyncCompositeCommand()
        {
            
        }

        public AsyncCompositeCommand(IList<IAsyncCommand> commands, Action<object> executedCallback)
        {
            this.commands = commands;
            this.executedCallback = executedCallback;
        }

        public void SetCallback(Action<object> executedCallback)
        {
            this.executedCallback = executedCallback;
        }

        public virtual void RegisterCommand(IAsyncCommand command)
        {
            this.commands.Add(command);
        }
        public virtual void UnregisterCommand(IAsyncCommand command)
        {
            this.commands.Remove(command);
        }

        public bool CanExecute(object parameter)
        {
            return commands.Any(x => x.CanExecute(parameter));
        }

        public async Task ExecuteAsync(object parameter)
        {
            var pendingTasks = commands.Select(c => c.ExecuteAsync(parameter))
                                        .ToList();
            await Task.WhenAll(pendingTasks);

            executedCallback(parameter);//Notify
        }

        public async Task ExecuteAsyncByQueue(object parameter)
        {
            foreach (var cmd in commands)
            {
                await cmd.ExecuteAsync(parameter);
            }

            executedCallback(parameter);//Notify
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
    }
}
