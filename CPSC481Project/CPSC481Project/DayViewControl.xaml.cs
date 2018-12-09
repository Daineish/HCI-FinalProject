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
        public AppointmentDatabase m_appointmentDatabase { get; set; }

        public DayViewControl(AppointmentDatabase a)
        {
            
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();
            m_appointmentDatabase = a;
            UpdateDayWithAppointments();
        }

        public DayViewControl(DateTime day, AppointmentDatabase ad)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add();

            m_day = day;
            currentDate.Content = m_day.ToString("dddd MMMM d, yyyy");
            //Setting proper Dayview Offset
            int offset = 8;
            if ((DateTime.Now.TimeOfDay.Hours - offset < 10) && (currentDate.Content.ToString() == DateTime.Now.ToString("dddd MMMM d, yyyy")))
            {
                int val = DateTime.Now.TimeOfDay.Hours - offset;
                dvScrollView.ScrollToVerticalOffset(val * 216);
            }
            else
            {
                dvScrollView.ScrollToVerticalOffset(0);
            }


            //--------
            m_appointmentDatabase = ad;
            UpdateDayWithAppointments();
        }

        // TODO: This is some DANK code. Literally trash, so clean it up pls.
        // TODO: When calling this, we should first revert back to the original screen
        private void UpdateDayWithAppointments()
        {
            ClearAppointments();
            DateTime cur = DateTime.Parse((String)currentDate.Content);
            foreach (Appointment a in m_appointmentDatabase.m_appointments)
            {
                DateTime appTime = a.m_startTime;
                int hr = appTime.Hour;
                if (cur.Year == appTime.Year && cur.Month == appTime.Month && cur.Day == appTime.Day && hr >= 8 && hr < 18)
                {
                    Grid gr = new Grid
                    {
                        Height = 36,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Rectangle rec = new Rectangle
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Stroke = new SolidColorBrush(Color.FromRgb(0,0,0)),
                        RadiusX = 10,
                        RadiusY = 10
                    };
                    TextBlock tb = new TextBlock
                    {
                        Text = a.m_patient.m_firstName + " " + a.m_patient.m_lastName,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Height = 36,
                        TextAlignment = TextAlignment.Center,
                        Padding = new Thickness(5),
                        FontSize = 18,
                        Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    };
                    tb.MouseUp += EditAppointment_Clicked;
                    gr.Children.Add(rec);
                    gr.Children.Add(tb);

                    if (a.m_doctor == "Dr. Walter")
                    {
                        List<StackPanel> panels = new List<StackPanel> { Walter8, Walter9, Walter10, Walter11, Walter12, Walter13, Walter14, Walter15, Walter16, Walter17 };
                        int index = -1;
                        foreach (StackPanel p in panels)
                        {
                            foreach (UIElement c in p.Children)
                            {
                                if (c.GetType() != typeof(Rectangle))
                                    continue;
                                Rectangle r = (Rectangle)c;

                                DateTime datetime = DateTime.Parse((String)currentDate.Content + " " + (String)r.Tag);
                                if (datetime.Hour != a.m_startTime.Hour)
                                {
                                    break;
                                }
                                else if (DateTime.Compare(datetime, a.m_startTime) == 0)
                                {
                                    index = p.Children.IndexOf(c);
                                    tb.Tag = r.Tag;
                                    break;
                                }
                            }
                            if (index != -1)
                            {
                                rec.Fill = new SolidColorBrush(Color.FromRgb(103, 103, 255));
                                p.Children.RemoveAt(index);
                                p.Children.Insert(index, gr);
                                break;
                            }
                        }
                    }
                    else if (a.m_doctor == "Dr. Lee")
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
                                DateTime datetime = DateTime.Parse((String)currentDate.Content + " " + (String)r.Tag);
                                if (datetime.Hour != a.m_startTime.Hour)
                                {
                                    break;
                                }
                                else if (DateTime.Compare(datetime, a.m_startTime) == 0)
                                {
                                    index = p.Children.IndexOf(c);
                                    tb.Tag = r.Tag;
                                    break;
                                }
                            }
                            if (index != -1)
                            {
                                rec.Fill = new SolidColorBrush(Color.FromRgb(234, 142, 122));
                                p.Children.RemoveAt(index);
                                p.Children.Insert(index, gr);
                                break;
                            }
                        }
                    }
                    else if (a.m_doctor == "Dr. Payne")
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
                                DateTime datetime = DateTime.Parse((String)currentDate.Content + " " + (String)r.Tag);
                                if (datetime.Hour != a.m_startTime.Hour)
                                {
                                    break;
                                }
                                else if (DateTime.Compare(datetime, a.m_startTime) == 0)
                                {
                                    index = p.Children.IndexOf(c);
                                    tb.Tag = r.Tag;
                                    break;
                                }
                            }
                            if (index != -1)
                            {
                                rec.Fill = new SolidColorBrush(Color.FromRgb(90, 170, 126));
                                p.Children.RemoveAt(index);
                                p.Children.Insert(index, gr);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void EditAppointment_Clicked(object sender, MouseButtonEventArgs e)
        {
            // bring up window to edit appointment
            EditAppointment form = new EditAppointment();
            TextBlock tb = (TextBlock)sender;
            //Grid g = (Grid)sender;
            //TextBlock tb = null;
            //foreach(UIElement c in g.Children)
            //{
            //    if (c.GetType() == typeof(TextBlock))
            //        tb = (TextBlock)c;
            //}
            //if (tb == null)
            //{
            //    MessageBox.Show("Error: Could not edit appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            Grid g = (Grid)tb.Parent;
            StackPanel sp = (StackPanel)g.Parent;
            DateTime dt = DateTime.Parse((String)currentDate.Content + " " + (String)tb.Tag);
            String doc = (String)sp.Tag;
            form.SetInfo(tb.Text, "patient number", (String)sp.Tag, dt);
            form.ShowDialog();
            if(form.m_delete)
            {
                // delete appointment
                bool success = false;
                foreach(Appointment a in m_appointmentDatabase.m_appointments)
                {
                    if(a.m_patient.m_firstName + " " + a.m_patient.m_lastName == tb.Text)
                    {
                        if(DateTime.Compare(a.m_startTime, dt) == 0)
                        {
                            m_appointmentDatabase.DeleteAppointment(a);
                            success = true;
                            break;
                        }
                    }
                }
                if (!success)
                {
                    MessageBox.Show("Error: Could not delete appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Appointment deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateDayWithAppointments();
                }
            }
            else if(form.m_changed)
            {
                // edit appointment
                Appointment ap = null;
                foreach (Appointment a in m_appointmentDatabase.m_appointments)
                {
                    if (a.m_patient.m_firstName + " " + a.m_patient.m_lastName == tb.Text)
                    {
                        if (DateTime.Compare(a.m_startTime, dt) == 0 && a.m_doctor == doc)
                        {
                            ap = a;
                            break;
                        }
                    }
                }
                if (ap == null)
                {
                    MessageBox.Show("Error: Could not edit appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bool conflict = false;
                    foreach (Appointment a in m_appointmentDatabase.m_appointments)
                    {
                        if (DateTime.Compare(a.m_startTime, form.m_startDate) == 0 && a.m_doctor == form.m_doctor)
                        {
                            conflict = true;
                            break;
                        }
                    }
                    if (conflict)
                    {
                        String msg = "Appointment time conflicts with another appointment, nothing was changed.";
                        MessageBox.Show(msg, "Time Conflict", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        ap.m_doctor = form.m_doctor;
                        ap.m_startTime = form.m_startDate;
                        MessageBox.Show("Appointment changed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateDayWithAppointments();
                    }
                    
                }
                
            }
        }

        public void ClearAppointments()
        {
            List<StackPanel> panels = new List<StackPanel> { Walter8, Walter9, Walter10, Walter11, Walter12, Walter13, Walter14, Walter15, Walter16, Walter17,
                                                             Lee8, Lee9, Lee10, Lee11, Lee12, Lee13, Lee14, Lee15, Lee16, Lee17,
                                                             Payne8, Payne9, Payne10, Payne11, Payne12, Payne13, Payne14, Payne15, Payne16, Payne17};

            Color color = Color.FromRgb(0, 0, 0);
            foreach (StackPanel p in panels)
            {
                List<int> removeThese = new List<int>();
                List<Rectangle> addThese = new List<Rectangle>();
                foreach (UIElement c in p.Children)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        Grid g = (Grid)c;
                        foreach(UIElement c1 in g.Children)
                        {
                            if(c1.GetType() == typeof(TextBlock))
                            {
                                TextBlock txt = (TextBlock)c1;

                                Rectangle r = new Rectangle
                                {
                                    Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                                    Height = 36,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                                    Tag = txt.Tag,
                                };
                                r.MouseUp += CreateAppointment_Click;
                                // width = auto
                                // fill = #6767ff, for ex.

                                removeThese.Add(p.Children.IndexOf(c));
                                addThese.Add(r);
                            }
                        }
                        

                    }
                }
                for(int i = 0; i < removeThese.Count(); i++)
                {
                    p.Children.RemoveAt(removeThese.ElementAt(i));
                    p.Children.Insert(removeThese.ElementAt(i), addThese.ElementAt(i));
                }
            }
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
            try
            {
                String[] newdate = miniCalendar.SelectedDate.ToString().Split(' ');
                String[] splitFormat = newdate[0].Split('/');
                int year = int.Parse(splitFormat[2]);
                int month = int.Parse(splitFormat[0]);
                int day = int.Parse(splitFormat[1]);
                DateTime date1 = new DateTime(year, month, day, 12, 0, 0);

                currentDate.Content = date1.ToString("dddd MMMM d, yyyy");
                //Scrollview Position
                int offset = 8;
                if ((DateTime.Now.TimeOfDay.Hours - offset < 10) && (currentDate.Content.ToString() == DateTime.Now.ToString("dddd MMMM d, yyyy")))
                {
                    int val = DateTime.Now.TimeOfDay.Hours - offset;
                    dvScrollView.ScrollToVerticalOffset(val * 216);
                }
                else
                {
                    dvScrollView.ScrollToVerticalOffset(0);
                }

                UpdateDayWithAppointments();
            }
            catch(Exception ex)
            {
                // Do nothing?
                Console.WriteLine("Caught exception: " + ex.Message);
            }

        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("Type: " + sender.GetType());
        }

        private void CreateAppointment_Click(object sender, MouseButtonEventArgs e)
        {
            // Bring up window to add appointment
            EditAppointment form = new EditAppointment();
            Rectangle r = (Rectangle)sender;
            DateTime datetime = DateTime.Parse((String)currentDate.Content + " " + (String)r.Tag);

            Grid g = (Grid)this.Parent;
            MainWindow mw = (MainWindow)g.Parent;
            m_appointmentDatabase = mw.NewAppointmentClicked(datetime, (String)((StackPanel)(r.Parent)).Tag);

            UpdateDayWithAppointments();
        }

        private void ToMonth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Grid g = (Grid)this.Parent;
            MainWindow mw = (MainWindow)g.Parent;
            mw.ToMonthView();
        }

        // TODO: Implemented drag & drop functionality for setting an appointment?
    }
}
