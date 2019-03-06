using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace WorkDelimiter.View
{
    /// <summary>
    /// Логика взаимодействия для AlarmView.xaml
    /// </summary>
    public partial class AlarmView : Window
    {
        public AlarmView()
        {
            InitializeComponent();
        }
        //private void closeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

        //Служит для перемещения окна за заголовок формы
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                //VistaGlassHelper.ExtendGlass(this, -1, -1, -1, -1);
            }
        }
    }
}
