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
    class TaskListViewModel: ViewModelBase
    {
        public DataContext dc;
        ObservableCollection<TaskOneTime> _listOneTime;
        ObservableCollection<TaskRegular> _listRegular;
        ITask _selectedTask;
        DialogService DS = new DialogService();
        public ObservableCollection<TaskOneTime> ListOneTime { get { return _listOneTime; } set { _listOneTime = value; this.RaisePropertyChanged("ListOneTime"); } }
        public ObservableCollection<TaskRegular> ListRegular { get { return _listRegular; } set { _listRegular = value; this.RaisePropertyChanged("ListRegular"); } }
        public ITask SelectedTask { get { return _selectedTask; } set { _selectedTask = value; this.RaisePropertyChanged("SelectedTask"); } }
        public TaskListViewModel()
        {
            dc = new Model.DataContext();
            Refresh();
        }
        public void Refresh()
        {
            ListOneTime = new ObservableCollection<TaskOneTime>(dc.ListTasksOneTime.Items);
            ListRegular = new ObservableCollection<TaskRegular>(dc.ListTasksRegular.Items);
        }
        void SaveDb()
        { 
            dc.UpdateChange(); 
        }
        void DeleteTask()
        {
            if (SelectedTask.GetType() == typeof(TaskOneTime))
            {       
                ListOneTime.Remove((TaskOneTime)SelectedTask);
                return;
            }
            if (SelectedTask.GetType() == typeof(TaskRegular))
                ListRegular.Remove((TaskRegular)SelectedTask);
        }
        void UpdateTimeRegular()
        {
            foreach (var t in ListRegular.Where(p => p.isActual == 1))
            {
                t.StartTime = System.DateTime.Now;
                t.DelimiterTime = t.StartTime.AddMinutes(t.period);
                t.NextTime = t.DelimiterTime.AddMinutes(t.delimiter);
            }
        }
        ITask NewTask(int t)
        {
            ITask newtask=null;
            switch (t)
            {
                case 1: newtask = new TaskOneTime(); break;
                case 2: newtask = new TaskRegular(); break;
                default: DS.ShowMessage("Unknow Task Type");break;
            }
            return newtask;
        }
        private ICommand _command;
        public ICommand ToggleActive
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        SaveDb();
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand AddTask
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        if (((TaskRegular)SelectedTask).id == 0)
                        {
                            int id = ListRegular.Max(t => t.id)+1;
                            ((TaskRegular)SelectedTask).id = id;
                            ListRegular.Add((TaskRegular)SelectedTask);
                        }
                    }, (object param) => { if(SelectedTask!=null) return true; else return false;}
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand StartRegular
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        UpdateTimeRegular();
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand DeleteSelectedTaskRegular
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        DeleteTask();
                    }, (object param) => { if(SelectedTask!=null) {if(SelectedTask.GetType()==typeof(TaskRegular)) return true; else return false;} else return false; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand DeleteSelectedTaskOneTime
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        DeleteTask();
                    }, (object param) => { if(SelectedTask!=null) {if(SelectedTask.GetType()==typeof(TaskOneTime)) return true; else return false;} else return false;}
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand NewTaskRegular
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        SelectedTask = NewTask(2);
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand NewTaskOneTime
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        SelectedTask = NewTask(1);
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
    }
}
