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
    /// Interaction logic for DayBoxAppointmentControl.xaml
    /// </summary>
    public partial class DayBoxAppointmentControl : UserControl
    {
        public DayBoxAppointmentControl()
        {
            InitializeComponent();
        }

        public void SetDoctor(String str)
        {
            if(str == "Dr. Payne")
            {
                BorderElement.BorderBrush = new SolidColorBrush(Color.FromRgb(90, 170, 126));
                BorderElement.Background = new SolidColorBrush(Color.FromRgb(90, 170, 126));
            }
            else if(str == "Dr. Lee")
            {
                BorderElement.BorderBrush = new SolidColorBrush(Color.FromRgb(234, 142, 122));
                BorderElement.Background = new SolidColorBrush(Color.FromRgb(234, 142, 122));
            }
            else if(str == "Dr. Walter")
            {
                BorderElement.BorderBrush = new SolidColorBrush(Color.FromRgb(103, 103, 255));
                BorderElement.Background = new SolidColorBrush(Color.FromRgb(103, 103, 255));
            }
        }
    }
}
