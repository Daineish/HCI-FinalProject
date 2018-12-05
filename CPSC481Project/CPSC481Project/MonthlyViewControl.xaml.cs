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
    /// Interaction logic for MonthlyViewControl.xaml
    /// </summary>
    public partial class MonthlyViewControl : UserControl
    {
        private List<Appointment> _myAppointmentsList = new List<Appointment>();
        //public MonthViewHeader aptCalendar = new MonthViewHeader();


        public MonthlyViewControl()
        {
            InitializeComponent();
            //this.MonthlyViewGrid.Children.Add(aptCalendar);
            Random rand = new Random(DateTime.Now.Second);

            for (int i = 1; i <= 50; i++)
            {
                Appointment apt = new Appointment();
                apt.AppointmentID = i;
                apt.StartTime = new DateTime(DateTime.Now.Year, rand.Next(1, 12), rand.Next(1, 28));
                apt.EndTime = apt.StartTime;
                apt.Subject = "Random apt, blah blah";
                _myAppointmentsList.Add(apt);
            }

            SetAppointments();
        }

        private void DayBoxDoubleClicked_event(NewAppointmentEventArgs e)
        {
            MessageBox.Show("You double-clicked on day " + e.StartDate.Value.ToShortDateString(), "Calendar Event", MessageBoxButton.OK);
        }

        private void AppointmentDblClicked(int Appointment_Id)
        {
            MessageBox.Show("You double-clicked on appointment with ID = " + Appointment_Id, "Calendar Event", MessageBoxButton.OK);
        }

        private void DisplayMonthChanged(MonthChangedEventArgs e)
        {
            SetAppointments();
        }

        
        private void SetAppointments()
        {
            // -- Use whatever function you want to load the MonthAppointments list, I happen to have a list filled by linq that has
            // many (possibly the past several years) of them loaded, so i filter to only pass the ones showing up in the displayed
            // month.  Note that the "setter" for MonthAppointments also triggers a redraw of the display.
            Predicate<Appointment> aptFind = delegate(Appointment apt)
            {
                //int temp = aptCal.DisplayStartDate.Month;
                return apt.StartTime.Value != null
                && (int)apt.StartTime.Value.Month == this.aptCalendar.DisplayStartDate.Month 
                && (int)apt.StartTime.Value.Year == this.aptCalendar.DisplayStartDate.Year;
            } ;
            List<Appointment> aptInDay = _myAppointmentsList.FindAll(aptFind);
            this.aptCalendar.MonthAppointments = aptInDay;
        }

        private void ToDashboard_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void ToDayView_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);
            w.MonthViewToDayView(DateTime.Today);
        }
    }
}
