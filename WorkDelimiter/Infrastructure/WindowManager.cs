using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkDelimiter.Infrastructure
{
    class NavigationItem
    {
        public ViewModelBase ViewModel;
        public Window View;
        delegate void OnClose();
        public NavigationItem(ViewModelBase vm, Window v)
        {
            View = v;
            ViewModel = vm;
        }
        public void Clear()
        {
            //код освобождения памяти от ViewModel и View
        }
    }
    public class WindowManager : IWindowManager
    {

        private static WindowManager instance;
        static List<NavigationItem> VVMs = new List<NavigationItem>();

        //List<NavigationItem> VVMs;
        //private WindowManager()
        //{
        //    VVMs = new List<NavigationItem>(); 
        //}

        public static WindowManager GetInstance()
        {
            if (instance == null)
                instance = new WindowManager();
            return instance;
        }
        public void ShowWindow(ViewModelBase viewModel)
        {
            NavigationItem newVVMItem = new NavigationItem(viewModel, CreateWindow(viewModel));
            newVVMItem.View.DataContext = viewModel;
            newVVMItem.View.Closing += viewModel.CloseNavigationElement;
            VVMs.Add(newVVMItem);
            newVVMItem.View.Show();
        }
        public void ActivateWindow(ViewModelBase viewModel)
        {

        }
        public void ShowModalDialog(ViewModelBase viewModel)
        {
            NavigationItem newVVMItem = new NavigationItem(viewModel, CreateWindow(viewModel));
            newVVMItem.View.DataContext = viewModel;
            newVVMItem.View.Closing += viewModel.CloseNavigationElement;
            VVMs.Add(newVVMItem);
            newVVMItem.View.ShowDialog();
        }
        public void ShowModalDialog(ViewModelBase viewModel, Action RemoteDelegate)
        {
            NavigationItem newVVMItem = new NavigationItem(viewModel, CreateWindow(viewModel));
            newVVMItem.View.DataContext = viewModel;
            newVVMItem.View.Closing += viewModel.CloseNavigationElement;
            VVMs.Add(newVVMItem);
            RemoteDelegate();
            newVVMItem.View.ShowDialog();
        }
        public void CloseWindow(ViewModelBase viewModel)
        {
            NavigationItem close = VVMs.FirstOrDefault(t => t.ViewModel == viewModel);
            try
            {
                close.View.Close();
                VVMs.Remove(close);
                close.Clear();
                close = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        private Window CreateWindow(ViewModelBase viewModel)
        {
            var modelType = viewModel.GetType();
            var windowTypeName
                = modelType.Name.Replace("ViewModel", "View"); // условие принятого соглашения именования. в данном случае "ViewModel - View"
            try
            {
                var windowTypes
                    = from t in modelType.Assembly.GetTypes()
                      where t.IsClass
                      && t.Name == windowTypeName
                      select t;
                return (Window)Activator.CreateInstance(windowTypes.Single());
            }
            catch
            {
                throw new Exception("The View is not found!");
            }
        }
    }
}
