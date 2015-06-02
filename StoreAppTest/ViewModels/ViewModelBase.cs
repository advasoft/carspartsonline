namespace StoreAppTest.ViewModels
{
    using System.ComponentModel;
    using Annotations;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract void LoadViewHandler();

        public void LoadView()
        {
            LoadViewHandler();
        }


    }
}
