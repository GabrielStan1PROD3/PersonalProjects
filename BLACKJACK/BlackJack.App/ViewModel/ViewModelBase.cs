using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlackJack.App.ViewModel;

public class ViewModelBase : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
    protected virtual void Notify(params string[] properties)
    {
        foreach (var p in properties)
            OnPropertyChanged(p);
    }

}
