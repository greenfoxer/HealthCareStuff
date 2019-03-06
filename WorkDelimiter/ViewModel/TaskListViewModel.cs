using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkDelimiter.Infrastructure;
using WorkDelimiter.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;

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
        List<string> Alarmed;
        public TaskListViewModel()
        {
            dc = new Model.DataContext();
            Refresh();
            Thread AlarmerOneTime = new Thread(new ThreadStart(AlarmOneTime));
            //Alarmer.SetApartmentState(ApartmentState.STA);

            AlarmerOneTime.IsBackground = true;
            AlarmerOneTime.Start();
            Thread AlarmerRegular = new Thread(new ThreadStart(AlarmRegular));
            //Alarmer.SetApartmentState(ApartmentState.STA);

            AlarmerRegular.IsBackground = true;
            AlarmerRegular.Start();
        }
        static object locker = new object();
        public void AlarmRegular()
        {
            while (true)
            {
                foreach (TaskRegular t in ListRegular.Where(t => t.isActual == 1&&t.IsTick==true))
                {
                    if (t.StartTime < System.DateTime.Now && t.DelimiterTime > System.DateTime.Now)
                    {
                        PushStatus = 1;
                        Mediator.GetInstance().NotifyColleagues("StatusWaiting", PushStatus);
                    }
                    if (t.DelimiterTime < System.DateTime.Now && t.NextTime > System.DateTime.Now)
                    {
                        PushStatus = 2;
                        Mediator.GetInstance().NotifyColleagues("StatusWaiting", PushStatus);
                    }
                    lock (locker)
                    {
                        ViewModelBase A = new AlarmViewModel();
                        if (!WindowManager.GetInstance().IsNavigationItemExist(A))
                        {
                            Thread thread = new Thread(() =>
                            {
                                WindowManager.GetInstance().ShowModalDialogHard(A);
                            });
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.IsBackground = true;
                            thread.Start();
                        }
                        Thread.Sleep(100);
                    }
                }
                Thread.Sleep(3000);
            }
        }
        int PushStatus;
        public void AlarmOneTime()
        { 
            while(true)
            {
                foreach (TaskOneTime t in ListOneTime.Where(t => t.isActual == 1))
                {
                    if (t.signalDate < System.DateTime.Now)
                    {
                        t.isActual = 0;
                        Mediator.GetInstance().Register("ModalDialogPrepared", PushTheValue);
                        Mediator.GetInstance().Register("CloseAlarmer", CloseAlarmer);
                        if (Alarmed == null)
                        {
                            Alarmed = new List<string>();
                            Alarmed.Add(t.name);
                        }
                        else
                            Alarmed.Add(t.name);
                        ViewModelBase A = new AlarmViewModel();
                        if (!WindowManager.GetInstance().IsNavigationItemExist(A))
                        {
                            Thread thread = new Thread(() =>
                            {
                                WindowManager.GetInstance().ShowModalDialogHard(A);
                            });
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.IsBackground = true;
                            thread.Start();
                        }
                        //System.Windows.Threading.Dispatcher.Run();
                    }
                }
                Thread.Sleep(3000);
            }
        }
        public void CloseAlarmer(object obj)
        {
            Alarmed = null;
        }
        public void PushTheValue(object obj)
        {
            Mediator.GetInstance().NotifyColleagues("AlarmIsWait", Alarmed);
        }
        public void PushTheStatus(object obj)
        {
            Mediator.GetInstance().NotifyColleagues("StatusWaiting", PushStatus);
        }
     
        #region innerlogic
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
            dc.DeleteTask(SelectedTask);
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
        void AddTaskToList()
        {
            if (SelectedTask.GetType() == typeof(TaskRegular))
            {
                if (((TaskRegular)SelectedTask).id == 0)
                {
                    int id = ListRegular.Max(t => t.id) + 1;
                    ((TaskRegular)SelectedTask).id = id;
                    ListRegular.Add((TaskRegular)SelectedTask);
                }
            }
            if (SelectedTask.GetType() == typeof(TaskOneTime))
            {
                if (((TaskOneTime)SelectedTask).id == 0)
                {
                    int id = ListRegular.Max(t => t.id) + 1;
                    ((TaskOneTime)SelectedTask).id = id;
                    ListOneTime.Add((TaskOneTime)SelectedTask);
                }
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
        #endregion
        #region commands
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
        public ICommand Decline
        {
            get
            {
                this._command = new RelayCommand(
                    (object param) =>
                    {
                        SelectedTask = null;
                    }, (object param) => { if(SelectedTask!=null) return true; else return false;}
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
                        AddTaskToList();
                        SelectedTask = null;
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
                        dc.UpdateTimeRegular();
                        RaisePropertyChanged("ListRegular");
                        //UpdateTimeRegular();
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
        #endregion
    }
}
