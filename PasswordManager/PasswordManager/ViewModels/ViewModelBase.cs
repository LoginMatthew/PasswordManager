
using System.ComponentModel;

namespace PasswordManager.ViewModels
{
    //It can be only used by inheritence
    //The INotifyPropertyChanged notify binding value about the property is changed and need to revalute the values
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //If parameter propertyName is null, then its raised a property changed and UI grabs every elements what is
        //binding to not just a specific property
        public virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Need to be virtual to override in the descandents classes
        public virtual void Dispose() { }
    }
}
