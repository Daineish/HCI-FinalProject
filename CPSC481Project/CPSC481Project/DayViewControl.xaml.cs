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
            currentDate.Content = m_day.ToString("dddd MMMM d, yyyy");
            Console.WriteLine(miniCalendar.SelectedDate);
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

        private void miniCalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(miniCalendar.SelectedDate);
            String[] newdate = miniCalendar.SelectedDate.ToString().Split(' ');
            Console.WriteLine(newdate[0]);
            String[] splitFormat = newdate[0].Split('/');
            int year = int.Parse(splitFormat[2]);
            int month = int.Parse(splitFormat[0]);
            int day = int.Parse(splitFormat[1]);
            DateTime date1 = new DateTime(year,month, day, 12, 0,0);

            currentDate.Content = date1.ToString("dddd MMMM d, yyyy");
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Type: " + sender.GetType());
        }

        private void CreateAppointment_Click(object sender, MouseButtonEventArgs e)
        {
            // TODO: Handle clicking on an already set appointment

            // Set new appointment
            Rectangle r = (Rectangle)sender;
            StackPanel p = (StackPanel)r.Parent;

            DateTime datetime = DateTime.Parse((String)currentDate.Content + " " + (String)r.Tag);
            String doc = (String)p.Tag;

            Console.WriteLine("Create an appointment at: " + datetime.ToString("f") + " with " + doc);
        }

        // TODO: Implemented drag & drop functionality for setting an appointment?
        // TODO: List appointments on dayview
    }
}
