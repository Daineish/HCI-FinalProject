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
        public DayViewControl()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();
        }

        private void OnBackButton(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            //dashboard.Visibility = Visibility.Visible;
        }


    }
}
