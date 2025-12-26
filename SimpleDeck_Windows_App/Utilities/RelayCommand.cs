using System;
using System.Windows.Input;

namespace SimpleDeck_Windows_App.Utilities;

public class RelayCommand(Action<object?> executer, Func<object?, bool>? canExecute = null)
    : ICommand
{
    private readonly Action<object?> _executer = executer;
    private readonly Func<object?, bool>? _canExecute = canExecute;
    
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    
    public void Execute(object? parameter) => _executer(parameter);
}