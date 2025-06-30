using System;
using System.Windows.Input;

namespace WPFPointApp.ViewModels
{
    internal class RelayCommand : ICommand
    {
        public bool CanExecute(object parameter) => m_CanExecute == null || m_CanExecute(parameter);
        public void Execute(object parameter) => m_Execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_CanExecute = canExecute;
        }

        private readonly Action<object> m_Execute;
        private readonly Func<object, bool> m_CanExecute;
    }
}
