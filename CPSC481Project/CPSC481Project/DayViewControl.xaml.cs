using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for DayViewControl.xaml
    /// </summary>
    public partial class DayViewControl : UserControl
    {
        public DateTime m_day { get; set; }
        public DayViewControl()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();
        }

        public DayViewControl(DateTime day)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();

            m_day = day;
            currentDate.Content = m_day.ToString("f");
        }

        private void OnBackButton(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            //dashboard.Visibility = Visibility.Visible;
        }

        private void ToDashboard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
