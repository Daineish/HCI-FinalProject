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
        //public MonthViewHeader aptCalendar = new MonthViewHeader();


        public MonthlyViewControl(VacationDatabase vd)
        {
            InitializeComponent();
            aptCalendar.DisplayMonthChanged += DisplayMonthChanged;
            aptCalendar.DayBoxDoubleClicked += DayBoxDoubleClicked_event;
            aptCalendar.AppointmentDblClicked += AppointmentDblClicked;
            //this.MonthlyViewGrid.Children.Add(aptCalendar);

            List<Vacation> temp = new List<Vacation>();
            foreach(Vacation v in vd.m_vacationList)
            {
                temp.Add(v);
            }
            foreach(Vacation v in temp)
            {
                DateTime dt = v.m_startDate;
                while(DateTime.Compare(dt, v.m_endDate) < 0)
                {
                    _myAppointmentsList.Add(new Vacation(v.m_doctor, v.m_startDate, v.m_endDate, dt));
                    dt = dt.AddDays(1);
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

        public void AppointmentDblClicked(String doc, DateTime start, DateTime end)
        {
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);

            EditVacation form = new EditVacation();
            form.SetInfo(doc, start, end);
            form.ShowDialog();
            if (form.m_delete)
            {
                List<Vacation> v1 = new List<Vacation>();
                DateTime cur = start;
                foreach (Vacation v in _myAppointmentsList)
                {
                    if (v.m_doctor == doc && DateTime.Compare(v.m_startDate, start) >= 0 && DateTime.Compare(v.m_endDate, end) <=0)
                    {
                        v1.Add(v);
                    }
                }
                if (v1.Count() == 0)
                {
                    // messagebox error
                    MessageBox.Show("Error: Could not delete appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    foreach (Vacation v in v1)
                    {
                        _myAppointmentsList.Remove(v); // and remove from database in MainWindow...?
                        w.m_vacationDatabase.RemoveVacation(v);
                    }
                    // success msg?
                    // update view
                    MessageBox.Show("Appointment deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetAppointments();
                }
            }
            else if (form.m_changed)
            {
                // edit appointment
                List<Vacation> v1 = new List<Vacation>();
                foreach (Vacation v in _myAppointmentsList)
                {
                    if (v.m_doctor == doc && DateTime.Compare(v.m_startDate, start) >= 0 && DateTime.Compare(v.m_endDate, end) <= 0)
                    {
                        v1.Add(v);
                    }
                }
                if (v1.Count() == 0)
                {
                    MessageBox.Show("Error: Could not edit appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // messagebox error
                }
                else
                {
                    foreach (Vacation v in v1)
                    {
                        _myAppointmentsList.Remove(v);
                        w.m_vacationDatabase.RemoveVacation(v);
                    }
                    w.m_vacationDatabase.AddVacation(new Vacation(form.m_doctor, form.m_startDate, form.m_endDate));

                    DateTime dt = form.m_startDate;
                    while (DateTime.Compare(dt, form.m_endDate) < 0)
                    {
                        _myAppointmentsList.Add(new Vacation(form.m_doctor, form.m_startDate, form.m_endDate, dt));
                        dt = dt.AddDays(1);
                    }

                    MessageBox.Show("Appointment changed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetAppointments();
                }
            }
        }

        private void DisplayMonthChanged(MonthChangedEventArgs e)
        {
            SetAppointments();
        }

        
        public void SetAppointments()
        {
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
            try
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
            catch(Exception e)
            {
                Console.WriteLine("Exception caught: " + e.Message);
                return true;
            }
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

        public void NewVacationWindow(String doc)
        {
            Grid g2 = (Grid)(this.Parent);
            MainWindow w = (MainWindow)(g2.Parent);

            AddVacation form = new AddVacation();
            form.SetInfo(doc);
            form.ShowDialog();

            if (form.m_add)
            {
                // add appointment
                w.m_vacationDatabase.AddVacation(new Vacation(form.m_doctor, form.m_startDate, form.m_endDate));

                DateTime dt = form.m_startDate;
                while (DateTime.Compare(dt, form.m_endDate) < 0)
                {
                    _myAppointmentsList.Add(new Vacation(form.m_doctor, form.m_startDate, form.m_endDate, dt));
                    dt = dt.AddDays(1);
                }

                MessageBox.Show("Appointment added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                SetAppointments();
            }
        }
    }
}
