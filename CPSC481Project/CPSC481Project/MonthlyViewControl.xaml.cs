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
                        _myAppointmentsList.Add(new Vacation(v.m_doctor, v.m_startDate, v.m_endDate, dt));
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

        private void AppointmentDblClicked(String doc, DateTime start, DateTime end)
        {
            //EditVacation form = new EditVacation();
            //form.SetInfo(doc, start, end);
            //form.ShowDialog();
            //if (form.m_delete)
            //{
            //    List<Vacation> v1 = null;
            //    DateTime cur = start;
            //    foreach(Vacation v in _myAppointmentsList)
            //    {
            //        if(v.m_doctor == doc && v.m_startDate == start && v.m_endDate == end)
            //        {
            //            v1 = v;
            //            break;
            //        }
            //    }
            //    if (v1 == null)
            //    {
            //        // messagebox error
            //        MessageBox.Show("Error: Could not delete appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        _myAppointmentsList.Remove(v1); // and remove from database in MainWindow...?
            //        // success msg?
            //        // update view
            //        MessageBox.Show("Appointment deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //        SetAppointments();
            //    }
            //}
            //else if (form.m_changed)
            //{
            //    // edit appointment
            //    Vacation v1 = null;
            //    foreach (Vacation v in _myAppointmentsList)
            //    {
            //        if (v.m_doctor == doc && v.m_startDate == start && v.m_endDate == end)
            //        {
            //            v1 = v;
            //            break;
            //        }
            //    }
            //    if (v1 == null)
            //    {
            //        MessageBox.Show("Error: Could not edit appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        // messagebox error
            //    }
            //    else
            //    {
                    
            //        v1.m_startDate = form.m_startDate;
            //        v1.m_endDate = form.m_startDate;
            //        v1.m_doctor = form.m_doctor;
            //        // edit database in MainWindow...
            //        // success msg?
            //        // update view
            //        MessageBox.Show("Appointment changed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //        SetAppointments();
            //    }
            //}
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
                return apt.m_curDate != null
                && (int)apt.m_curDate.Month == this.aptCalendar.DisplayStartDate.Month 
                && (int)apt.m_curDate.Year == this.aptCalendar.DisplayStartDate.Year
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
