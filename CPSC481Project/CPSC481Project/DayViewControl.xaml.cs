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
using System.Windows.Markup;
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

        public DayViewControl(AppointmentDatabase a)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();

            UpdateDayWithAppointments(a);
        }

        // TODO: This is some DANK code. Literally trash, so clean it up pls. Also I think the time's it's showing have an off-by-one error so fix that too.
        // TODO: also call this when adding an appointment so the view is updated right away.
        private void UpdateDayWithAppointments(AppointmentDatabase ad)
        {
            DateTime cur = DateTime.Parse((String)currentDate.Content);
            foreach (Appointment a in ad.m_appointments)
            {
                DateTime appTime = a.m_startTime;
                if (cur.Year == appTime.Year && cur.Month == appTime.Month && cur.Day == appTime.Day)
                {
                    int hr = appTime.Hour;
                    if(hr >= 8 && hr < 18)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Text = a.m_patient.m_firstName + " " + a.m_patient.m_lastName;
                        tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                        tb.VerticalAlignment = VerticalAlignment.Top;
                        tb.Height = 36;
                        Console.WriteLine("Adding appointment with: " + tb.Text + " doc: " + a.m_doctor + " at " + a.m_startTime.ToString("t"));
                        if (a.m_doctor == "Dr. Walter")
                        {
                            List<StackPanel> panels = new List<StackPanel> { Walter8, Walter9, Walter10, Walter11, Walter12, Walter13, Walter14, Walter15, Walter16, Walter17 };
                            int index = -1;
                            foreach(StackPanel p in panels)
                            {
                                foreach(UIElement c in p.Children)
                                {
                                    if (c.GetType() != typeof(Rectangle))
                                        continue;
                                    Rectangle r = (Rectangle)c;
                                    
                                    DateTime datetime = DateTime.Parse((String)r.Tag);
                                    if(datetime.Hour != a.m_startTime.Hour)
                                    {
                                        break;
                                    }
                                    else if(DateTime.Compare(datetime, a.m_startTime) <= 0 && DateTime.Compare(datetime.AddMinutes(10), a.m_startTime) >= 0)
                                    {
                                        index = p.Children.IndexOf(c);
                                        break;
                                    }
                                }
                                if(index != -1)
                                {
                                    tb.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                                    p.Children.RemoveAt(index);
                                    p.Children.Insert(index, tb);
                                    break;
                                }
                            }
                        }
                        else if(a.m_doctor == "Dr. Lee")
                        {
                            List<StackPanel> panels = new List<StackPanel> { Lee8, Lee9, Lee10, Lee11, Lee12, Lee13, Lee14, Lee15, Lee16, Lee17 };
                            int index = -1;
                            foreach (StackPanel p in panels)
                            {
                                foreach (UIElement c in p.Children)
                                {
                                    if (c.GetType() != typeof(Rectangle))
                                        continue;
                                    Rectangle r = (Rectangle)c;
                                    DateTime datetime = DateTime.Parse((String)r.Tag);
                                    if (datetime.Hour != a.m_startTime.Hour)
                                    {
                                        break;
                                    }
                                    else if (DateTime.Compare(datetime, a.m_startTime) <= 0 && DateTime.Compare(datetime.AddMinutes(10), a.m_startTime) >= 0)
                                    {
                                        index = p.Children.IndexOf(c);
                                        break;
                                    }
                                }
                                if (index != -1)
                                {
                                    tb.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                                    p.Children.RemoveAt(index);
                                    p.Children.Insert(index, tb);
                                    break;
                                }
                            }
                        }
                        else if(a.m_doctor == "Dr. Payne")
                        {
                            List<StackPanel> panels = new List<StackPanel> { Payne8, Payne9, Payne10, Payne11, Payne12, Payne13, Payne14, Payne15, Payne16, Payne17 };
                            int index = -1;
                            foreach (StackPanel p in panels)
                            {
                                foreach (UIElement c in p.Children)
                                {
                                    if (c.GetType() != typeof(Rectangle))
                                        continue;
                                    Rectangle r = (Rectangle)c;
                                    DateTime datetime = DateTime.Parse((String)r.Tag);
                                    if (datetime.Hour != a.m_startTime.Hour)
                                    {
                                        break;
                                    }
                                    else if (DateTime.Compare(datetime, a.m_startTime) <= 0 && DateTime.Compare(datetime.AddMinutes(10), a.m_startTime) >= 0)
                                    {
                                        index = p.Children.IndexOf(c);
                                        break;
                                    }
                                }
                                if (index != -1)
                                {
                                    tb.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                                    p.Children.RemoveAt(index);
                                    p.Children.Insert(index, tb);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public DayViewControl(DateTime day, AppointmentDatabase ad)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();

            m_day = day;
            currentDate.Content = m_day.ToString("dddd MMMM d, yyyy");
            Console.WriteLine(miniCalendar.SelectedDate);

            UpdateDayWithAppointments(ad);
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
            Grid g = (Grid)this.Parent;
            MainWindow mw = (MainWindow)g.Parent;
            mw.NewAppointmentClicked(datetime, doc);

        }

        // TODO: Implemented drag & drop functionality for setting an appointment?
        // TODO: List appointments on dayview
    }
}
