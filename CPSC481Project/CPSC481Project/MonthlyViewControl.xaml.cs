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

using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;


namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for MonthlyViewControl.xaml
    /// </summary>
    public partial class MonthlyViewControl : UserControl
    {
        private List<Vacation> _myAppointmentsList = new List<Vacation>();
        private static String m_vacationFile = "./Vacations.txt";
        //public MonthViewHeader aptCalendar = new MonthViewHeader();


        public MonthlyViewControl()
        {
            InitializeComponent();
            aptCalendar.DisplayMonthChanged += DisplayMonthChanged;
            aptCalendar.DayBoxDoubleClicked += DayBoxDoubleClicked_event;
            aptCalendar.AppointmentDblClicked += AppointmentDblClicked;
            //this.MonthlyViewGrid.Children.Add(aptCalendar);
            Random rand = new Random(DateTime.Now.Second);

            if (!File.Exists(m_vacationFile))
            {
                // idk if this is necessary because idk how files work in C#.
                File.Create(m_vacationFile).Close();
            }

            StreamReader vacationFile = File.OpenText(m_vacationFile);

            JsonSerializer serializer = new JsonSerializer();
            List<Vacation> temp = (List<Vacation>)(serializer.Deserialize(vacationFile, typeof(List<Vacation>)));
            if (temp == null)
            {
                temp = new List<Vacation>();
            }
            else
            {
                foreach(Vacation v in temp)
                {
                    DateTime dt = v.m_startDate;
                    while(DateTime.Compare(dt, v.m_endDate) < 0)
                    {
                        _myAppointmentsList.Add(new Vacation(v.m_doctor, dt, dt));
                        dt = dt.AddDays(1);
                    }
                }
            }

            //SetAppointments();
        }

        ~MonthlyViewControl()
        {
            //String json = JsonConvert.SerializeObject(_myAppointmentsList, Formatting.Indented);
            //File.WriteAllText(m_vacationFile, json);
        }

        private void DayBoxDoubleClicked_event(NewAppointmentEventArgs e)
        {
            // open day view
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);
            w.MonthViewToDayView(e.StartDate.GetValueOrDefault());
        }

        private void AppointmentDblClicked(int Appointment_Id)
        {
            MessageBox.Show("You double-clicked on appointment with ID = " + Appointment_Id, "Calendar Event", MessageBoxButton.OK);
        }

        private void DisplayMonthChanged(MonthChangedEventArgs e)
        {
            SetAppointments();
        }

        
        public void SetAppointments()
        {
            // -- Use whatever function you want to load the MonthAppointments list, I happen to have a list filled by linq that has
            // many (possibly the past several years) of them loaded, so i filter to only pass the ones showing up in the displayed
            // month.  Note that the "setter" for MonthAppointments also triggers a redraw of the display.
            Predicate<Vacation> aptFind = delegate(Vacation apt)
            {
                //int temp = aptCal.DisplayStartDate.Month;
                return apt.m_startDate != null
                && (int)apt.m_startDate.Month == this.aptCalendar.DisplayStartDate.Month 
                && (int)apt.m_startDate.Year == this.aptCalendar.DisplayStartDate.Year
                && DoctorChecked(apt.m_doctor);
            } ;
            List<Vacation> aptInDay = _myAppointmentsList.FindAll(aptFind);
            this.aptCalendar.MonthAppointments = aptInDay;
        }

        private bool DoctorChecked(String doc)
        {
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);
            if (doc == "Dr. Payne")
                return w.paynecBox.IsChecked != null ? w.paynecBox.IsChecked.Value : false;
            else if (doc == "Dr. Lee")
                return w.leecBox.IsChecked != null ? w.leecBox.IsChecked.Value : false;
            else if (doc == "Dr. Walter")
                return w.waltercBox.IsChecked != null ? w.waltercBox.IsChecked.Value : false;
            return false;
        }

        private void ToDashboard_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);
            w.PatientListScrollViewer.Height = 708;
            w.filterDoctor.Visibility = Visibility.Hidden;
            
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
