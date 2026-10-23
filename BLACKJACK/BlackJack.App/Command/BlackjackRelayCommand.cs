using System.Windows.Input;

namespace BlackJack.App.Command;

class BlackjackRelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    
    //private readonly Action _execute;
    //private readonly Func<bool>? _canExecute;

    public BlackjackRelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }


    // can work also manually with RaiseCanExecuteChanged();
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }


    //public void RaiseCanExecuteChanged() 
    //{
    //    CanExecute?.Invoke(this, EventArgs.Empty);
    //}

}
