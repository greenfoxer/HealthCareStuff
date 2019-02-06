using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkDelimiter.Infrastructure
{
    interface IWindowManager
    {
        void ShowWindow(ViewModelBase viewModel);
        void ActivateWindow(ViewModelBase viewModel);
        void ShowModalDialog(ViewModelBase viewModel);
        void CloseWindow(ViewModelBase viewModel);
    }
}
