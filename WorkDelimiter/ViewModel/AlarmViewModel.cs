using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkDelimiter.Infrastructure;
using WorkDelimiter.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace WorkDelimiter.ViewModel
{
    class AlarmViewModel:ViewModelBase
    {
        ObservableCollection<string> _currentAlarm;
        public ObservableCollection<string> CurrentAlarm { get { return _currentAlarm; } set { _currentAlarm = value; RaisePropertyChanged("CurrentAlarm"); } }

        public AlarmViewModel()
        {
            Mediator.GetInstance().Register("AlarmIsWait", OnRefreshList);
            Mediator.GetInstance().Register("StatusWaiting", StatusWaiting);
            Mediator.GetInstance().NotifyColleagues("ModalDialogPrepared", null);
        }
        public void OnRefreshList(object obj)
        {
            if(obj!=null)
                CurrentAlarm = new ObservableCollection<string>((List<string>)obj);
        }
        int _status;
        public int Status { get { return _status; } set { _status = value; RaisePropertyChanged("Status"); } }

        private ICommand _command;
        public ICommand CloseWindow
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        WindowManager.GetInstance().CloseWindow(this);
                        Mediator.GetInstance().NotifyColleagues("CloseAlarmer", null);
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public void StatusWaiting(object obj)
        { 
            Status = (int)obj;
        }
    }
}
