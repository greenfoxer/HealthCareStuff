﻿using System;
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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WorkDelimiter.View
{
    /// <summary>
    /// Логика взаимодействия для TaskListView.xaml
    /// </summary>
    public partial class TaskListView : Window
    {
        public TaskListView()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

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
