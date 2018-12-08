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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for DrLeeTileControl.xaml
    /// </summary>
    public partial class DrLeeTileControl : UserControl
    {
        public DrLeeTileControl()
        {
            InitializeComponent();
        }

        private void ToVacay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid g2 = (Grid)(this.Parent);
            Grid g3 = (Grid)(g2.Parent);
            MainWindow w = (MainWindow)(g3.Parent);
            w.ToVacayCalendar("Dr. Lee");
        }
    }
}
