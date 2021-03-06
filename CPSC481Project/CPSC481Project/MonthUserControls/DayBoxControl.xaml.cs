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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for DayBoxControl.xaml
    /// </summary>
    /// 

    
    public partial class DayBoxControl : UserControl
    {
        public int m_day { get; set; }

        public DayBoxControl()
        {
            InitializeComponent();
            // error
        }

        public DayBoxControl(int i)
        {
            InitializeComponent();
            m_day = i;
        }

        public void Highlight()
        {
            DayAppointmentsStack.Background = new SolidColorBrush(Color.FromRgb(50, 0, 0));
        }
    }
}
