using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkDelimiter.Infrastructure
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //protected bool SetProperty(ref T storage, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (object.Equals(storage, value)) 
        //        return false;
        //    storage = value;
        //    this.RaisePropertyChanged(propertyName);
        //    return true;
        //}
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (this.PropertyChanged != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
            //if (this.PropertyChanged != null)
            //{
            //    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
        public virtual void CloseNavigationElement(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = e.Cancel;//true - отмена закрытия окна
        }
    }
}
