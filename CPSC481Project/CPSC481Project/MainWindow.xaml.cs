using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using static CPSC481Project.DrPayneTileControl;
//using static CPSC481Project.DrLeeTileControl;
//using static CPSC481Project.DrWalterTileControl;



namespace CPSC481Project
{
    enum Doctor
    {
        payne, lee, walter
    };
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        String[] names = { "Jason", "Marge", "Eddie", "Nicky", "Alex", "Jason", "Jessy", "Sally", "Misty", "Sam", "Ron", "Ted", "Jenny", "Nancy", "Sophia", "Steve", "Adam", "Kyle", "Kevin", "Ricky", "Eunice", "Jane", "Mike", "Noah", "William", "David", "Mary", "Patricia", "Linda", "Barbara", "Susan", "Elizabeth" };
        String[] lastnames = { "Adam", "Johnson", "Williamson", "Smith", "Wong", "Nguyen", "Tran", "Lee", "Isaac", "Liamson", "Ronald", "Donald", "McDonald", "Davidson", "Larson" };
        String[] doctors = { "Dr.Lee", "Dr.Walter", "Dr.Payne" };
        String[] domain = { "@hotmail.com", "@gmail.com", "@gmail.ca", "@yahoo.ca", "@yahoo.com", "@shaw.ca", "@gmail.ca", "@live.ca", "@live.com" };
        String[] addr = { "University", "Edgemont", "Hamptons", "Hawkwood", "Citadel", "Sandstone", "Country", "Ave", "Downtown" };
        String[] addr2 = { "Street", "Drive", "Memorial", "Dale", "Rise", "Mews", "Place", "Bank" };
        String[] month = { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        
        internal DateTime _DisplayStartDate = DateTime.Now;
        Boolean filterMode = false;
        Patient m_currentPatient;
        Boolean selectedMode;
        PatientDatabase m_patientDatabase;
        AppointmentDatabase m_appointmentDatabase;
        public VacationDatabase m_vacationDatabase { get; set; }
        DayViewControl m_dayViewControl;
        MonthlyViewControl m_monthlyViewControl;
        Boolean m_recentPatientsShowing;
        Patient m_payneNext1;
        Patient m_payneNext2;
        Patient m_leeNext1;
        Patient m_leeNext2;
        Patient m_walterNext1;
        Patient m_walterNext2;

        //for walkinqueue
        private int queuePosition = 0;
        public ObservableCollection<WalkinTile> _walkinList = new ObservableCollection<WalkinTile>();

        public MainWindow()
        {
            setTimer();
            InitializeComponent();
            m_patientDatabase = new PatientDatabase();
            m_appointmentDatabase = new AppointmentDatabase();
            m_vacationDatabase = new VacationDatabase();
            m_recentPatientsShowing = true;
            
            //get current date for dashboard
            DashDate.Content = month.ElementAt(_DisplayStartDate.Month - 1) + " " + _DisplayStartDate.Day + ", " + _DisplayStartDate.Year;

            // Add data for demo if not already present in cache/database files
            if (m_patientDatabase.NumPatients() < 5)
            {
                m_patientDatabase.AddPatient(new Patient("Smith", "John", "00001", "769 1st St. NW", "john.smith@yahoo.ca", "(403) 819-0193"));
                m_patientDatabase.AddPatient(new Patient("Johnson", "Phillip", "15432", "12 3rd Ave. SW", "pjohnson@company.com", "(403) 543-4189"));
                m_patientDatabase.AddPatient(new Patient("Bobson", "Bob", "15795", "Unknown", "bob@bob.ca", "(555) 555-7777"));
                m_patientDatabase.AddPatient(new Patient("Albertson", "Albert", "11325", "1600 Pennsylvania Ave", "al@hotmail.com", "(444) 645-1234 ext. 4"));
                m_patientDatabase.AddPatient(new Patient("Trump", "Don", "12345", "1600 Pennsylvania Ave", "YoloDonny420@whitehouse.gov", "(555) 867-5309"));
                m_patientDatabase.AddPatient(new Patient("Twain", "Mark", "44444", "5 Mississippi River", "huck.finn@live.com", "Unknown"));
                m_patientDatabase.AddPatient(new Patient("Smith", "Mark", "00002", "2500 University Ave", "president@ucalgary.ca", "(403) 999-9999"));
                m_patientDatabase.AddPatient(new Patient("Adultman", "Vincent", "99999", "412 Adult St.", "adultman_v@bigcompany.com", "(555) 555-5555"));
                m_patientDatabase.AddPatient(new Patient("Griffin", "Peter", "83409", "31 Spooner St.", "pgriffin@drunkenclam.com", "(123) 456-7890"));
            }

            if (m_appointmentDatabase.NumAppointments() < 5)
            {
                DateTime today = DateTime.Today;
                TimeSpan ts = new TimeSpan(15, 0, 0);
                today = today.Date + ts;
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00001"), "Dr. Walter", today, today.AddMinutes(10), "Appointment #1"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("12345"), "Dr. Payne", today.AddMinutes(30), today.AddMinutes(40), "Appointment #2"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00002"), "Dr. Lee", today.AddDays(1), today.AddDays(1).AddMinutes(10), "Appointment #3"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("44444"), "Dr. Walter", today.AddHours(1), today.AddHours(4).AddMinutes(10), "Appointment #4"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00001"), "Dr. Walter", today.AddMinutes(-20), today.AddMinutes(-10), "Appointment #5"));
            }

            if(m_vacationDatabase.NumVacations() < 3)
            {
                DateTime s1 = DateTime.Parse("2019-01-01");
                DateTime e1 = DateTime.Parse("2019-02-01");
                DateTime s2 = DateTime.Parse("2019-02-05");
                DateTime e2 = DateTime.Parse("2019-02-12");
                DateTime s3 = DateTime.Parse("2018-12-24");
                DateTime e3 = DateTime.Parse("2019-01-02");
                m_vacationDatabase.AddVacation(new Vacation("Dr. Walter", s1, e1));
                m_vacationDatabase.AddVacation(new Vacation("Dr. Lee", s2, e2));
                m_vacationDatabase.AddVacation(new Vacation("Dr. Payne", s3, e3));
            }

            // Debug - show all patients and appointments in console.
            m_appointmentDatabase.PrintAllAppointments();
            m_patientDatabase.PrintAllPatients();
            m_vacationDatabase.PrintAllVacations();
            //randomPatientGenerator();

            PopulateDefaultInfo();

            //for walkinqueuelist
            _walkinList.Add(new WalkinTile("John Doe", "12333", 1, _walkinList, walkinQueueList));
            _walkinList.Add(new WalkinTile("John Deer", "33455", 2, _walkinList, walkinQueueList));
            _walkinList.Add(new WalkinTile("Jane Deer", "33465", 3, _walkinList, walkinQueueList));

            //walkinQueueList.DisplayMemberPath = "Name";//*
            walkinQueueList.ItemsSource = _walkinList;
            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(walkinqueue_PreviewMouseMoveEvent)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(walkinqueue_Drop)));
            walkinQueueList.ItemContainerStyle = itemContainerStyle;
        }

        /**
         * Used to populate the main screen with expected data for the demo.
         * 
         * TODO: This isn't great code and could be cleaned up.
         */
        private void PopulateDefaultInfo()
        {
            // Get next appointments for each doctor from database
            List<Appointment> appointmentPayne1 = m_appointmentDatabase.NextAppointments("Dr. Payne", 2);
            List<Appointment> appointmentLee1 = m_appointmentDatabase.NextAppointments("Dr. Lee", 2);
            List<Appointment> appointmentWalter1 = m_appointmentDatabase.NextAppointments("Dr. Walter", 2);
            for(int i = 0; i < 2 && i < appointmentPayne1.Count(); i++)
            {
                Appointment app = appointmentPayne1.ElementAt(i);
                Patient pat = appointmentPayne1.ElementAt(i).m_patient;
                if (i == 0 && pat != null)
                {
                    this.DoctorPayneTile.npfullName.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorPayneTile.npfullName.MouseDoubleClick += NextPatientClicked;
                    this.DoctorPayneTile.npfullName.Cursor = Cursors.Hand;
                    
                    m_payneNext1 = pat;
                }
                if (i == 1 && pat != null)
                {
                    this.DoctorPayneTile.npfullName2.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorPayneTile.npfullName2.MouseDoubleClick += NextPatientClicked;
                    this.DoctorPayneTile.npfullName2.Cursor = Cursors.Hand;
                    m_payneNext2 = pat;
                }
            }
            for (int i = 0; i < 2 && i < appointmentLee1.Count(); i++)
            {
                Appointment app = appointmentLee1.ElementAt(i);
                Patient pat = appointmentLee1.ElementAt(i).m_patient;
                if (i == 0 && pat != null)
                {
                    this.DoctorLeeTile.npfullName.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorLeeTile.npfullName.MouseDoubleClick += NextPatientClicked;
                    this.DoctorLeeTile.npfullName.Cursor = Cursors.Hand;
                    m_leeNext1 = pat;
                }
                if (i == 1 && pat != null)
                {
                    this.DoctorLeeTile.npfullName2.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorLeeTile.npfullName2.MouseDoubleClick += NextPatientClicked;
                    this.DoctorLeeTile.npfullName2.Cursor = Cursors.Hand;
                    m_leeNext2 = pat;
                }
            }
            for (int i = 0; i < 2 && i < appointmentWalter1.Count(); i++)
            {
                Appointment app = appointmentWalter1.ElementAt(i);
                Patient pat = appointmentWalter1.ElementAt(i).m_patient;
                if (i == 0 && pat != null)
                {
                    this.DoctorWalterTile.npfullName.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorWalterTile.npfullName.MouseDoubleClick += NextPatientClicked;
                    this.DoctorWalterTile.npfullName.Cursor = Cursors.Hand;
                    m_walterNext1 = pat;
                }
                if (i == 1 && pat != null)
                {
                    this.DoctorWalterTile.npfullName2.Content = app.m_startTime.ToString("t") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                    this.DoctorWalterTile.npfullName2.MouseDoubleClick += NextPatientClicked;
                    this.DoctorWalterTile.npfullName2.Cursor = Cursors.Hand;
                    m_walterNext2 = pat;
                }
            }

            if(!selectedMode)
            {
                // Populate recent patient list with fake data (TODO)
                Patient recent1 = m_patientDatabase.findPatient("15432");
                Patient recent2 = m_patientDatabase.findPatient("15795");
                Patient recent3 = m_patientDatabase.findPatient("11325");
                Patient recent4 = m_patientDatabase.findPatient("99999");
                Patient recent5 = m_patientDatabase.findPatient("83409");

                PatientListStackPanel.Children.Add(CreateGrid(recent1, true));
                PatientListStackPanel.Children.Add(CreateGrid(recent2, true));
                PatientListStackPanel.Children.Add(CreateGrid(recent3, true));
                PatientListStackPanel.Children.Add(CreateGrid(recent4, true));
                PatientListStackPanel.Children.Add(CreateGrid(recent5, true));
            }
            

            // Populate available times
            List<String> availablePayne = m_appointmentDatabase.AvailableTimes("Dr. Payne");
            List<String> availableLee = m_appointmentDatabase.AvailableTimes("Dr. Lee");
            List<String> availableWalter = m_appointmentDatabase.AvailableTimes("Dr. Walter");

            this.DoctorPayneTile.availTime1.MouseDoubleClick += AvailableTimeClicked;
            this.DoctorPayneTile.availTime2.MouseDoubleClick += AvailableTimeClicked;

            this.DoctorLeeTile.availTime1.MouseDoubleClick += AvailableTimeClicked;
            this.DoctorLeeTile.availTime2.MouseDoubleClick += AvailableTimeClicked;

            this.DoctorWalterTile.availTime1.MouseDoubleClick += AvailableTimeClicked;
            this.DoctorWalterTile.availTime2.MouseDoubleClick += AvailableTimeClicked;

            for (int i = 0; i < 2 && i < availablePayne.Count(); i++)
            {
                String str = availablePayne.ElementAt(i);
                if (i == 0 && str != null)
                {
                    this.DoctorPayneTile.availTime1.Content = str;
                    if (str.Equals("No appointments scheduled."))
                        {
                        
                        }
                    else
                        this.DoctorPayneTile.availTime1.Cursor = Cursors.Hand;
                }
                else if (i == 1 && str != null)
                {
                    this.DoctorPayneTile.availTime2.Content = str;
                    if (str.Equals("No appointments scheduled."))
                    {

                    }
                    else
                        this.DoctorPayneTile.availTime2.Cursor = Cursors.Hand;
                }
            }
            for (int i = 0; i < 2 && i < availableLee.Count(); i++)
            {
                String str = availableLee.ElementAt(i);
                if (i == 0 && str != null)
                {
                    this.DoctorLeeTile.availTime1.Content = str;
                    if (str.Equals("No appointments scheduled."))
                    {

                    }
                    else
                        this.DoctorLeeTile.availTime1.Cursor = Cursors.Hand;
                }
                else if (i == 1 && str != null)
                {
                    this.DoctorLeeTile.availTime2.Content = str;
                    if (str.Equals("No appointments scheduled."))
                    {

                    }
                    else
                        this.DoctorLeeTile.availTime2.Cursor = Cursors.Hand;
                }
            }
            for (int i = 0; i < 2 && i < availableWalter.Count(); i++)
            {
                String str = availableWalter.ElementAt(i);
                if (i == 0 && str != null)
                {
                    this.DoctorWalterTile.availTime1.Content = str;
                    if (str.Equals("No appointments scheduled."))
                    {

                    }
                    else
                        this.DoctorWalterTile.availTime1.Cursor = Cursors.Hand;
                }
                else if (i == 1 && str != null)
                {
                    this.DoctorWalterTile.availTime2.Content = str;
                    if (str.Equals("No appointments scheduled."))
                    {

                    }
                    else
                        this.DoctorWalterTile.availTime2.Cursor = Cursors.Hand;
                }
            }
        }

        private void setTimer()
        {
            Timer tmr = new Timer(1000);
            //count = times;
            tmr.Enabled = true;
            tmr.Elapsed += tmr_Elapsed;
            tmr.AutoReset = true;
            //tmr.Start();
        }

        //change the label text inside the tick event
        private void tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            int hour = DateTime.Now.Hour;
            String apm = "AM";
            if (hour > 12)
            {
                hour = hour - 12;
                apm = "PM";
            }

            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    int min = DateTime.Now.Minute;
                    String str = "";
                    if (min < 10) str = "0";
                    this.DashTime.Content = hour + ":" + str + DateTime.Now.Minute + " " + apm;
                    this.DashDate.Content = month.ElementAt(DateTime.Now.Month - 1) + " " + DateTime.Now.Day + ", " + DateTime.Now.Year;
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

        }

        private void searchClicked(object sender, MouseButtonEventArgs e)
        {
            searchField.Text = "";
            searchField.Opacity = 100;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // first clear StackPanel contents
            PatientListStackPanel.Children.Clear();
            recentLabel.Content = "Patient Search";
            m_recentPatientsShowing = false;

            // Add a button to remove search results
            RemoveSearchButton.Visibility = Visibility.Visible;

            if (IsDigitsOnly(searchField.Text))
            {
                List<Patient> patients = m_patientDatabase.FindPatientHC(searchField.Text);
                double size = 0;
                foreach (Patient pat in patients)
                {
                    Grid g = CreateGrid(pat, true, false);
                    size += g.Height;
                    PatientListStackPanel.Children.Add(g);
                }
                PatientListStackPanel.Height = size + 10;

            }
            else// if(Regex.IsMatch(searchField.Text, @"^[a-zA-Z]+$")) //Search by Lastname
            {
                List<Patient> patients = m_patientDatabase.FindPatientName(searchField.Text);
                foreach (Patient pat in patients)
                {
                    PatientListStackPanel.Children.Add(CreateGrid(pat, true, false));
                }
            }
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        bool IsLettersOnly(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }

        private void rpatientEnter(object sender, MouseEventArgs e)
        {

        }

        private void rpatientExit(object sender, MouseEventArgs e)
        {

        }

        private void searchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            // first clear StackPanel contents
            PatientListStackPanel.Children.Clear();
            recentLabel.Content = "Patient Search";
            m_recentPatientsShowing = false;

            // Add a button to remove search results
            RemoveSearchButton.Visibility = Visibility.Visible;

            if (IsDigitsOnly(searchField.Text))
            {
                List<Patient> patients = m_patientDatabase.FindPatientHC(searchField.Text);
                double size = 0;
                foreach (Patient pat in patients)
                {
                    Grid g = CreateGrid(pat, true, false);
                    size += g.Height;
                    PatientListStackPanel.Children.Add(g);
                }
                PatientListStackPanel.Height = size + 10;

            }
            else// if(Regex.IsMatch(searchField.Text, @"^[a-zA-Z]+$")) //Search by Lastname
            {
                List<Patient> patients = m_patientDatabase.FindPatientName(searchField.Text);
                foreach (Patient pat in patients)
                {
                    PatientListStackPanel.Children.Add(CreateGrid(pat, true, false));
                }
            }
        }

        /**
         * When searching for a patient, this method can be called to create a grid showing a patient's information.
         */
        private Grid CreateGrid(Patient p, Boolean withButton = false, Boolean highlighted = false)
        {
            int offset = -5;
            Grid g = new Grid();
            Label nameL = new Label();
            Label hcL = new Label();
            Label addyL = new Label();
            Label emailL = new Label();
            Label phoneL = new Label();
            Label prevdrL = new Label();
            //Only add grid if patient is not null
            if (p != null)
            {
                nameL.Content = "Name:"; nameL.Height = 40; nameL.Margin = new Thickness(16, 5, 0, 0); nameL.Width = 101; nameL.FontSize = 18; nameL.HorizontalAlignment = HorizontalAlignment.Left; nameL.VerticalAlignment = VerticalAlignment.Top;
                hcL.Content = "HC#:"; hcL.Height = 40; hcL.Margin = new Thickness(16, 45, 0, 0); hcL.Width = 101; hcL.FontSize = 18; hcL.HorizontalAlignment = HorizontalAlignment.Left; hcL.VerticalAlignment = VerticalAlignment.Top;
                addyL.Content = "Address:"; addyL.Height = 40; addyL.Margin = new Thickness(16, 85  , 0, 0); addyL.Width = 101; addyL.FontSize = 18; addyL.HorizontalAlignment = HorizontalAlignment.Left; addyL.VerticalAlignment = VerticalAlignment.Top;
                emailL.Content = "Email:"; emailL.Height = 40; emailL.Margin = new Thickness(16, 125 , 0, 0); emailL.Width = 101; emailL.FontSize = 18; emailL.HorizontalAlignment = HorizontalAlignment.Left; emailL.VerticalAlignment = VerticalAlignment.Top;
                phoneL.Content = "Phone:"; phoneL.Height = 40; phoneL.Margin = new Thickness(16, 165 , 0, 0); phoneL.Width = 101; phoneL.FontSize = 18; phoneL.HorizontalAlignment = HorizontalAlignment.Left; phoneL.VerticalAlignment = VerticalAlignment.Top;
                prevdrL.Content = "Prev Dr:"; prevdrL.Height = 40; prevdrL.Margin = new Thickness(16, 205 , 0, 0); prevdrL.Width = 140; prevdrL.FontSize = 18; prevdrL.HorizontalAlignment = HorizontalAlignment.Left; prevdrL.VerticalAlignment = VerticalAlignment.Top;


                TextBlock nameC = new TextBlock();
                TextBlock hcC = new TextBlock();
                TextBlock addyC = new TextBlock();
                TextBlock emailC = new TextBlock();
                TextBlock phoneC = new TextBlock();

                TextBlock prevdrC = new TextBlock();
                nameC.Text = p.GetLastName() + ", " + p.GetFirstName(); nameC.Height = 40; nameC.Margin = new Thickness(100, 10, 0, 0); nameC.Width = 220; nameC.FontSize = 18; nameC.FontWeight = FontWeights.Bold; nameC.HorizontalAlignment = HorizontalAlignment.Left; nameC.VerticalAlignment = VerticalAlignment.Top; nameC.TextTrimming = TextTrimming.CharacterEllipsis;
                hcC.Text = p.GetHCNumber(); hcC.Height = 40; hcC.Margin = new Thickness(100, 50, 0, 0); hcC.Width = 220; hcC.FontSize = 18; hcC.FontWeight = FontWeights.Bold; hcC.HorizontalAlignment = HorizontalAlignment.Left; hcC.VerticalAlignment = VerticalAlignment.Top; hcC.TextTrimming = TextTrimming.CharacterEllipsis;
                addyC.Text = p.GetAddress(); addyC.Height = 40; addyC.Margin = new Thickness(100, 90, 0, 0); addyC.Width = 220; addyC.FontSize = 18; addyC.FontWeight = FontWeights.Bold; addyC.HorizontalAlignment = HorizontalAlignment.Left; addyC.VerticalAlignment = VerticalAlignment.Top; addyC.TextTrimming = TextTrimming.CharacterEllipsis;
                emailC.Text = p.GetEmail(); emailC.Height = 40; emailC.Margin = new Thickness(100, 130, 0, 0); emailC.Width = 220; emailC.FontSize = 18; emailC.FontWeight = FontWeights.Bold; emailC.HorizontalAlignment = HorizontalAlignment.Left; emailC.VerticalAlignment = VerticalAlignment.Top; emailC.TextTrimming = TextTrimming.CharacterEllipsis;
                phoneC.Text = p.GetPhone(); phoneC.Height = 40; phoneC.Margin = new Thickness(100, 170, 0, 0); phoneC.Width = 220; phoneC.FontSize = 18; phoneC.FontWeight = FontWeights.Bold; phoneC.HorizontalAlignment = HorizontalAlignment.Left; phoneC.VerticalAlignment = VerticalAlignment.Top; phoneC.TextTrimming = TextTrimming.CharacterEllipsis;
                prevdrC.Text = p.GetPrev(); prevdrC.Height = 40; prevdrC.Margin = new Thickness(100, 210, 0, 0); prevdrC.Width = 220; prevdrC.FontSize = 18; prevdrC.FontWeight = FontWeights.Bold; prevdrC.HorizontalAlignment = HorizontalAlignment.Left; prevdrC.VerticalAlignment = VerticalAlignment.Top; prevdrC.TextTrimming = TextTrimming.CharacterEllipsis;

                Rectangle r = new Rectangle();
                r.Stroke = System.Windows.Media.Brushes.LightGray;
                r.HorizontalAlignment = HorizontalAlignment.Left;
                r.VerticalAlignment = VerticalAlignment.Top;
                r.Width = 320;
                r.Height = 285;
                Thickness p_margin = new Thickness();
                p_margin.Left = 2;
                p_margin.Right = 45;
                p_margin.Top = 2;
                p_margin.Bottom = 2;
                r.Margin = p_margin;
                r.RadiusX = 16.5;
                r.RadiusY = 16.5;
                r.StrokeThickness = 3;


                if (withButton)
                {
                    Button b = new Button();
                    b.Content = "Select";
                    b.Width = 100;
                    b.Height = 30;
                    b.VerticalAlignment = VerticalAlignment.Bottom;
                    b.Margin = new Thickness(-50, 0, 0, 15);
                    b.Tag = p;
                    b.Cursor = Cursors.Hand;
                    b.Click += SelectPatientClicked;
                    g.Children.Add(b);
                }
                else if (highlighted)
                {
                    r.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
                }

                //Add button to view patient information
                Button view = new Button();
                view.Content = "View";
                view.Width = 75;
                view.Height = 30;
                view.VerticalAlignment = VerticalAlignment.Bottom;
                view.Margin = new Thickness(150, 0, 0, 15);
                view.Tag = p;
                view.Cursor = Cursors.Hand;
                view.Click += selectViewPatient;
                g.Children.Add(view);

                g.Children.Add(r);
                g.Children.Add(nameL); g.Children.Add(hcL); g.Children.Add(addyL); g.Children.Add(emailL); g.Children.Add(phoneL); g.Children.Add(prevdrL) ;
                g.Children.Add(nameC); g.Children.Add(hcC); g.Children.Add(addyC); g.Children.Add(emailC); g.Children.Add(phoneC); g.Children.Add(prevdrC);

                //Now add an unselect button
                if (!withButton && highlighted)
                {
                    Button unselectPatient = new Button();
                    unselectPatient.Content = "Unselect";
                    unselectPatient.Width = 100;
                    unselectPatient.Height = 30;
                    unselectPatient.Margin = new Thickness(-50, 200, 0, 0);
                    unselectPatient.Tag = p;
                    unselectPatient.Cursor = Cursors.Hand;
                    unselectPatient.Click += unSelectPatientClicked;
                    g.Children.Add(unselectPatient);
                }
            }
            return g;
        }

        /**
         * The handler of when the "select patient" button is clicked.
         */
        private void SelectPatientClicked(object sender, RoutedEventArgs e)
        {
            addWalkInButton_grey.Visibility = Visibility.Hidden;
            addWalkInButton.Visibility = Visibility.Visible;
            Button b = (Button)sender;

            // Remove all boxes, then add back the selected patient box.
            PatientListStackPanel.Children.Clear();
            m_currentPatient = (Patient)b.Tag;
            PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, false, true));

            selectedMode = true;
        }

        private void unSelectPatientClicked(object sender, RoutedEventArgs e)
        {
            addWalkInButton_grey.Visibility = Visibility.Visible;
            addWalkInButton.Visibility = Visibility.Hidden;
            PatientListStackPanel.Children.Clear();
            //Set patient to null so we don't create another grid of the patient
            PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, true, false));
            m_currentPatient = null;
            selectedMode = false;

            if (m_recentPatientsShowing)
                PopulateDefaultInfo(); // or something
        }

        //View patient information
        private void selectViewPatient(object sender, RoutedEventArgs e)
        {
            //First, put all the required information in
            Button view = (Button)sender;

            PatientListStackPanel.Children.Clear();
            viewPatient.Visibility = Visibility.Visible;

            m_currentPatient = (Patient)view.Tag;
            dPatientname.Text = m_currentPatient.GetFirstName();
            dPatientlname.Text = m_currentPatient.GetLastName();
            dPatientHC.Text = m_currentPatient.GetHCNumber();
            dPatientaddr.Text = m_currentPatient.GetAddress();
            dPatientpn.Text = m_currentPatient.GetPhone();
            dPatientemail.Text = m_currentPatient.GetEmail();
            dPatientprevdr.Text = m_currentPatient.GetPrev();

            // TODO: Also, need to fix the "recent" label and add an exit button to exit out of viewing patient info
            recentLabel.Content = "View Patient:";
            //Reuse the same button as for exiting search
            RemoveSearchButton.Visibility = Visibility.Visible;

            // TODO: Future work: Add an edit button to edit any field
            editInfo.Visibility = Visibility.Visible;


        }

        //Edit patient info
        private void EditInfo_Click(object sender, RoutedEventArgs e)
        {
            //First, hide the text fields
            dPatientname.Visibility = Visibility.Hidden;
            dPatientlname.Visibility = Visibility.Hidden;
            dPatientHC.Visibility = Visibility.Hidden;
            dPatientaddr.Visibility = Visibility.Hidden;
            dPatientpn.Visibility = Visibility.Hidden;
            dPatientemail.Visibility = Visibility.Hidden;
            dPatientprevdr.Visibility = Visibility.Hidden;
            dPatientHCBox.IsEnabled = false;
            dPatientprevdrBox.IsEnabled = false;
            //Keep text the same, let users change if they want
            dPatientnameBox.Text = dPatientname.Text;
            dPatientlnameBox.Text = dPatientlname.Text;
            dPatientHCBox.Text = dPatientHC.Text;
            dPatientaddrBox.Text = dPatientaddr.Text;
            dPatientpnBox.Text = dPatientpn.Text;
            dPatientemailBox.Text = dPatientemail.Text;
            dPatientprevdrBox.Text = dPatientprevdr.Text;
            //Then show the text boxes
            dPatientnameBox.Visibility = Visibility.Visible;
            dPatientlnameBox.Visibility = Visibility.Visible;
            dPatientHCBox.Visibility = Visibility.Visible;
            dPatientaddrBox.Visibility = Visibility.Visible;
            dPatientpnBox.Visibility = Visibility.Visible;
            dPatientemailBox.Visibility = Visibility.Visible;
            dPatientprevdrBox.Visibility = Visibility.Visible;
            //Show the yes and cancel button
            editInfoYes.Visibility = Visibility.Visible;
            editInfoCancel.Visibility = Visibility.Visible;
        }

        private void EditInfoYes_Click(object sender, RoutedEventArgs e)
        {
            //Keep text the same, let users change if they want
            dPatientname.Text = dPatientnameBox.Text;
            dPatientlname.Text = dPatientlnameBox.Text;
            dPatientHC.Text = dPatientHCBox.Text;
            dPatientaddr.Text = dPatientaddrBox.Text;
            dPatientpn.Text = dPatientpnBox.Text;
            dPatientemail.Text = dPatientemailBox.Text;
            dPatientprevdr.Text = dPatientprevdrBox.Text;
            //Now hide the text boxes and make text fields visible
            dPatientnameBox.Visibility = Visibility.Hidden;
            dPatientlnameBox.Visibility = Visibility.Hidden;
            dPatientHCBox.Visibility = Visibility.Hidden;
            dPatientaddrBox.Visibility = Visibility.Hidden;
            dPatientpnBox.Visibility = Visibility.Hidden;
            dPatientemailBox.Visibility = Visibility.Hidden;
            dPatientprevdrBox.Visibility = Visibility.Hidden;
            
            dPatientname.Visibility = Visibility.Visible;
            dPatientlname.Visibility = Visibility.Visible;
            dPatientHC.Visibility = Visibility.Visible;
            dPatientaddr.Visibility = Visibility.Visible;
            dPatientpn.Visibility = Visibility.Visible;
            dPatientemail.Visibility = Visibility.Visible;
            dPatientprevdr.Visibility = Visibility.Visible;
            //Replace and edit information.
          
            Patient p = m_patientDatabase.findPatient(dPatientHCBox.Text.ToString());
            if(p != null)
            {
                p.m_firstName = dPatientnameBox.Text;
                p.m_lastName = dPatientlnameBox.Text;
                p.m_phoneNumber = dPatientpnBox.Text;
                p.m_prevDr = dPatientprevdrBox.Text;
                p.m_email = dPatientemailBox.Text;
                p.m_address = dPatientaddrBox.Text;
            }
            /*
            Patient p = new Patient(dPatientlnameBox.Text, dPatientnameBox.Text, dPatientHCBox.Text, dPatientaddrBox.Text, dPatientemailBox.Text, dPatientpnBox.Text, dPatientprevdrBox.Text);
            if(m_patientDatabase.findPatient(p.m_hcNumber)!= null)
            {
                m_patientDatabase.RemovePatient(p.m_hcNumber);
                m_patientDatabase.AddPatient(p);
            }
            */

            //Now hide the yes/cancel buttons
            editInfoYes.Visibility = Visibility.Hidden;
            editInfoCancel.Visibility = Visibility.Hidden;

            editInfo.Visibility = Visibility.Visible;
        }

        private void EditInfoCancel_Click(object sender, RoutedEventArgs e)
        {
            //If cancel, just make text boxes no longer visible
            dPatientnameBox.Visibility = Visibility.Hidden;
            dPatientlnameBox.Visibility = Visibility.Hidden;
            dPatientHCBox.Visibility = Visibility.Hidden;
            
            dPatientaddrBox.Visibility = Visibility.Hidden;
            dPatientpnBox.Visibility = Visibility.Hidden;
            dPatientemailBox.Visibility = Visibility.Hidden;
            dPatientprevdrBox.Visibility = Visibility.Hidden;
            //Then make text fields visible
            dPatientname.Visibility = Visibility.Visible;
            dPatientlname.Visibility = Visibility.Visible;
            dPatientHC.Visibility = Visibility.Visible;
            dPatientaddr.Visibility = Visibility.Visible;
            dPatientpn.Visibility = Visibility.Visible;
            dPatientemail.Visibility = Visibility.Visible;
            dPatientprevdr.Visibility = Visibility.Visible;
            //Now hide the yes/cancel buttons
            editInfoYes.Visibility = Visibility.Hidden;
            editInfoCancel.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //dashboard.Visibility = Visibility.Hidden;

            m_dayViewControl = new DayViewControl(DateTime.Today, m_appointmentDatabase);
            MainGrid.Children.Add(m_dayViewControl);
            Grid.SetRow(m_dayViewControl, 0);
            Grid.SetColumn(m_dayViewControl, 1);
            Grid.SetRowSpan(m_dayViewControl, 3);
            m_dayViewControl.Visibility = Visibility.Visible;
        }
        /*private void Back_Click(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Visible;
            dayview.Visibility = Visibility.Hidden;
        }*/

        private void calendarBack(object sender, RoutedEventArgs e)
        {   if(filterMode == true)
            {
                filterMode = false;
                dashboard.Visibility = Visibility.Visible;
                PatientListScrollViewer.Height = 708.5;
                filterDoctor.Visibility = Visibility.Hidden;
            }

            if (m_monthlyViewControl != null)
                MainGrid.Children.Remove(m_monthlyViewControl);
            // error checking?
        }
        private void ToCalendar_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            ToMonthView();
        }

        public void ToMonthView()
        {
            if (filterMode == false || filterDoctor.Visibility == Visibility.Hidden)
            {
                filterMode = true;
                PatientListScrollViewer.Height = 708.5 - 180.5;
                filterDoctor.Visibility = Visibility.Visible;
            }

            //dashboard.Visibility = Visibility.Hidden;
            m_monthlyViewControl = new MonthlyViewControl(m_vacationDatabase);
            m_monthlyViewControl.Visibility = Visibility.Visible;
            MainGrid.Children.Add(m_monthlyViewControl);
            Grid.SetRow(m_monthlyViewControl, 0);
            Grid.SetColumn(m_monthlyViewControl, 1);
            Grid.SetRowSpan(m_monthlyViewControl, 3);
            m_monthlyViewControl.SetAppointments();

            leecBox.IsChecked = true;
            waltercBox.IsChecked = true;
            paynecBox.IsChecked = true;
        }

        public void ToVacayCalendar(string drName)
        {
            // Auto check the desired doctor (is this what we want?)
            if(drName == "Dr. Lee")
            {
                leecBox.IsChecked = true;
                waltercBox.IsChecked = false;
                paynecBox.IsChecked = false;
            }
            else if(drName == "Dr. Walter")
            {
                leecBox.IsChecked = false;
                waltercBox.IsChecked = true;
                paynecBox.IsChecked = false;
            }
            else if(drName == "Dr. Payne")
            {
                leecBox.IsChecked = false;
                waltercBox.IsChecked = false;
                paynecBox.IsChecked = true;
            }

            if (filterMode == false || filterDoctor.Visibility == Visibility.Hidden)
            {
                filterMode = true;
                PatientListScrollViewer.Height = 708.5 - 180.5;
                filterDoctor.Visibility = Visibility.Visible;
            }

            m_monthlyViewControl = new MonthlyViewControl(m_vacationDatabase);
            m_monthlyViewControl.Visibility = Visibility.Visible;
            MainGrid.Children.Add(m_monthlyViewControl);
            Grid.SetRow(m_monthlyViewControl, 0);
            Grid.SetColumn(m_monthlyViewControl, 1);
            Grid.SetRowSpan(m_monthlyViewControl, 3);
            m_monthlyViewControl.SetAppointments();
            m_monthlyViewControl.NewVacationWindow(drName);
        }

        private void ToDayView_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            //dashboard.Visibility = Visibility.Hidden;
            if(filterMode == true)
            {
                filterDoctor.Visibility = Visibility.Hidden;
                PatientListScrollViewer.Height = 708.5;
                filterMode = false;
            }


            m_dayViewControl = new DayViewControl(DateTime.Today, m_appointmentDatabase);
            MainGrid.Children.Add(m_dayViewControl);
            Grid.SetRow(m_dayViewControl, 0);
            Grid.SetColumn(m_dayViewControl, 1);
            Grid.SetRowSpan(m_dayViewControl, 3);
            m_dayViewControl.Visibility = Visibility.Visible;
        }

        //Popup
        private void addContact(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Add Contact", "Add Contact");
            clearAddtext();
            recentLabel.Content = "Add Patient:";
            recentRec.Fill = (SolidColorBrush) Application.Current.Resources["AddPatient"];
            PatientListScrollViewer.Visibility = Visibility.Hidden;
            this.addPatient.Visibility = Visibility.Visible;
        }

        private void patientaddConfirm(object sender, RoutedEventArgs e)
        {

            //Error Handling can be put here.. to check if the strings are in correct format...
            //IE: Name should not have numbers....
            warning.Visibility = Visibility.Hidden;
            PatientListScrollViewer.Visibility = Visibility.Visible;
            String newName = apnameField.Text;
            String newLname = aplnameField.Text;
            String newHC = aphcField.Text;
            String newPhone = apphoneField.Text;
            String email = apemailField.Text;
            String addr = apaddrField.Text;

            //Add error check for names (letters only)
            if (!IsLettersOnly(newName) || !IsLettersOnly(newLname))
            {
                warning.Text = "Please ensure that names only contain letters";
                warning.Visibility = Visibility.Visible;
            }
            //Add error check for phone numbers and health care numbers (digits only)
            else if (!IsDigitsOnly(newHC))
            {
                warning.Text = "Please ensure that the Health Care # only contains digits";
                warning.Visibility = Visibility.Visible;
            }
            //else if (!IsDigitsOnly(newPhone))
            //{
            //    warning.Text = "Please ensure that the phone number only contains digits";
            //    warning.Visibility = Visibility.Visible;
            //}
            else if (newName == "" || newLname == "" || newHC == "" || newPhone == "" || email == "" || addr == "")
            {
                warning.Text = "Please ensure all required fields are completed.";
                warning.Visibility = Visibility.Visible;
            }
            else
            {
                PatientListScrollViewer.Visibility = Visibility.Hidden;
                //confirmBtn.Visibility = Visibility.Hidden;
                Yes.Visibility = Visibility.Visible;
                warning.Visibility = Visibility.Visible;
                warning.Text = "Are you sure?";

            }
        }

        private void cancelClicked(object sender, RoutedEventArgs e)
        {
            addPatient.Visibility = Visibility.Hidden;
            PatientListScrollViewer.Visibility = Visibility.Visible;
            warning.Visibility = Visibility.Hidden;
            //Cleartext
            clearAddtext();
        }

        private void patientConfirmed(object sender, RoutedEventArgs e)
        {
            String newName = apnameField.Text;
            String newLname = aplnameField.Text;
            String newHC = aphcField.Text;
            String newPhone = apphoneField.Text;
            String email = apemailField.Text;
            String addr = apaddrField.Text;

            //Now add the patient


            Patient p = new Patient(newName, newLname, newHC, addr, email, newPhone);
            warning.Visibility = Visibility.Hidden;
            Yes.Visibility = Visibility.Hidden;
            addPatient.Visibility = Visibility.Hidden;
            PatientListScrollViewer.Visibility = Visibility.Visible;
            m_patientDatabase.AddPatient(p);
            clearAddtext();

            //Patient p = new Patient(lastName, firstName, hcNum, address, email, phone);
        }

        public void clearAddtext()
        {
            apnameField.Text = "";
            aplnameField.Text = "";
            aphcField.Text = "";
            apphoneField.Text = "";
            apemailField.Text = "";
            apaddrField.Text = "";
            recentLabel.Content = "Recent Patients:";
            m_recentPatientsShowing = true;
            recentRec.Fill = (SolidColorBrush)Application.Current.Resources["RecentPatient"];
        }

        public AppointmentDatabase NewAppointmentClicked(DateTime datetime, String doc)
        {
            if (m_currentPatient == null)
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                AddAppointment form = new AddAppointment();
                form.SetInfo(m_currentPatient.m_firstName + " " + m_currentPatient.m_lastName, m_currentPatient.m_hcNumber, doc, datetime);
                form.ShowDialog();

                if (form.m_add)
                {
                    // create appointment
                    Appointment newApp = new Appointment(m_currentPatient, form.m_doctor, form.m_startDate, form.m_startDate, "");

                    bool conflict = false;
                    bool inPast = false;
                    foreach (Appointment a in m_appointmentDatabase.m_appointments)
                    {
                        if (DateTime.Compare(a.m_startTime, newApp.m_startTime) == 0 && a.m_doctor == newApp.m_doctor)
                        {
                            conflict = true;
                            break;
                        }
                    }
                    if (DateTime.Compare(newApp.m_startTime, DateTime.Now) < 0)
                    {
                        inPast = true;
                    }
                    if (conflict)
                    {
                        String msg = "Appointment time conflicts with another appointment, nothing was added.";
                        MessageBox.Show(msg, "Time Unavailable", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if(inPast)
                    {
                        String msg = "The appointment you are trying to add is in the past, please try another time.";
                        MessageBox.Show(msg, "Time Unavailable", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        m_appointmentDatabase.AddAppointment(newApp);
                        PopulateDefaultInfo();
                        MessageBox.Show("Appointment added", "Appointment added", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            return m_appointmentDatabase;
        }

        private void WalkInClicked(object sender, RoutedEventArgs e)
        {
            if (m_currentPatient == null)
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string temp_hc = m_currentPatient.GetHCNumber();
                bool in_walkinlist = false;
                foreach (WalkinTile t2 in _walkinList)
                {
                    string temp_hc2 = t2.HCLabel.ToString();
                    string[] temp_hc3 = temp_hc2.Split(' ');
                    Console.WriteLine(temp_hc + " " + temp_hc3[1]);
                    if (temp_hc == temp_hc3[1]) in_walkinlist = true;
                }
                if(in_walkinlist)
                {
                    MessageBox.Show("Selected patient is already in queue. ", "Duplicate Queue Entry", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    queuePosition++;
                    WalkinTile t = new WalkinTile(m_currentPatient.m_firstName + " " + m_currentPatient.m_lastName, m_currentPatient.m_hcNumber, queuePosition, _walkinList, walkinQueueList);
                    _walkinList.Add(t);
                    walkinQueueList.ItemsSource = _walkinList;
                    //walkinQueueList.Items.Add(t);
                }
            }
        }

        private void walkinqueue_PreviewMouseMoveEvent(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void walkinqueue_Drop(object sender, DragEventArgs e)
        {
            WalkinTile droppedData = e.Data.GetData(typeof(WalkinTile)) as WalkinTile;
            WalkinTile target = ((ListBoxItem)(sender)).DataContext as WalkinTile;

            int removedIdx = walkinQueueList.Items.IndexOf(droppedData);
            int targetIdx = walkinQueueList.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                _walkinList.Insert(targetIdx + 1, droppedData);
                _walkinList.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (_walkinList.Count + 1 > remIdx)
                {
                    _walkinList.Insert(targetIdx, droppedData);
                    _walkinList.RemoveAt(remIdx);
                }
            }
        }

        public void walkin_delete(WalkinTile a)
        {
            _walkinList.Remove(_walkinList.Where(i => i.HCLabel == a.HCLabel).Single());
            walkinQueueList.ItemsSource = _walkinList;
        }

        public void MonthViewToDayView(DateTime d)
        {
            if (filterMode == true)
            {
                filterDoctor.Visibility = Visibility.Hidden;
                PatientListScrollViewer.Height = 708.5;
                filterMode = false;
            }
            m_dayViewControl = new DayViewControl(d, m_appointmentDatabase);
            m_dayViewControl.Visibility = Visibility.Visible;
            MainGrid.Children.Add(m_dayViewControl);
            Grid.SetRow(m_dayViewControl, 0);
            Grid.SetColumn(m_dayViewControl, 1);
            Grid.SetRowSpan(m_dayViewControl, 3);
            if (m_monthlyViewControl != null)
                MainGrid.Children.Remove(m_monthlyViewControl);
            // error handling?
        }


        //Remove the example when user clicks into field
        private void ApnameField_GotFocus(object sender, RoutedEventArgs e)
        {
            apnameField.Text = "";
        }

        //Remove the example when user clicks into field
        private void AplnameField_GotFocus(object sender, RoutedEventArgs e)
        {
            aplnameField.Text = "";
        }

        //Remove the example when user clicks into field
        private void AphcField_GotFocus(object sender, RoutedEventArgs e)
        {
            aphcField.Text = "";
        }

        //Remove the example when user clicks into field
        private void ApaddrField_GotFocus(object sender, RoutedEventArgs e)
        {
            apaddrField.Text = "";
        }

        //Remove the example when user clicks into field
        private void ApphoneField_GotFocus(object sender, RoutedEventArgs e)
        {
            apphoneField.Text = "";
        }

        //Remove the example when user clicks into field
        private void ApemailField_GotFocus(object sender, RoutedEventArgs e)
        {
            apemailField.Text = "";
        }

        private void OnRemoveSearchButton(object sender, RoutedEventArgs e)
        {
            PatientListStackPanel.Children.Clear();
            PopulateDefaultInfo();
            recentLabel.Content = "Recent Patients: ";
            m_recentPatientsShowing = true;
            RemoveSearchButton.Visibility = Visibility.Hidden;
            selectedMode = false;
            m_currentPatient = null;

            viewPatient.Visibility = Visibility.Hidden;
            PopulateDefaultInfo();

            //This is for if its in EDIT MODE AND WE CLICK THE X
            //If cancel, just make text boxes no longer visible
            dPatientnameBox.Visibility = Visibility.Hidden;
            dPatientlnameBox.Visibility = Visibility.Hidden;
            dPatientHCBox.Visibility = Visibility.Hidden;

            dPatientaddrBox.Visibility = Visibility.Hidden;
            dPatientpnBox.Visibility = Visibility.Hidden;
            dPatientemailBox.Visibility = Visibility.Hidden;
            dPatientprevdrBox.Visibility = Visibility.Hidden;
            //Then make text fields visible
            dPatientname.Visibility = Visibility.Visible;
            dPatientlname.Visibility = Visibility.Visible;
            dPatientHC.Visibility = Visibility.Visible;
            dPatientaddr.Visibility = Visibility.Visible;
            dPatientpn.Visibility = Visibility.Visible;
            dPatientemail.Visibility = Visibility.Visible;
            dPatientprevdr.Visibility = Visibility.Visible;
            //Now hide the yes/cancel buttons
            editInfoYes.Visibility = Visibility.Hidden;
            editInfoCancel.Visibility = Visibility.Hidden;
        }
        //Filter Checked
        private void payneChecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }

        private void leeChecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }

        private void walterChecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }

        private void payneUnchecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }

        private void leeUnchecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }

        private void walterUnchecked(object sender, RoutedEventArgs e)
        {
            if (m_monthlyViewControl != null)
                m_monthlyViewControl.SetAppointments();
        }
        //Filter Unchecked

        private void NextPatientClicked(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() != typeof(Label))
                return;

            Label l = (Label)sender;
            Patient p = (l == this.DoctorPayneTile.npfullName) ? m_payneNext1 :
                        (l == this.DoctorPayneTile.npfullName2) ? m_payneNext2 :
                        (l == this.DoctorLeeTile.npfullName) ? m_leeNext1 :
                        (l == this.DoctorLeeTile.npfullName2) ? m_leeNext2 :
                        (l == this.DoctorWalterTile.npfullName) ? m_walterNext1 :
                        (l == this.DoctorWalterTile.npfullName2) ? m_walterNext2 : null;

            if (p != null)
            {
                // Remove all boxes, then add back the selected patient box.
                PatientListStackPanel.Children.Clear();
                m_currentPatient = (Patient)p;
                PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, false, true));

                selectedMode = true;
            }
        }

        private void AvailableTimeClicked(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() != typeof(Label))
                return;

            Label l = (Label)sender;
            String s = (l == this.DoctorPayneTile.availTime1) ? (String)this.DoctorPayneTile.availTime1.Content :
                        (l == this.DoctorPayneTile.availTime2) ? (String)this.DoctorPayneTile.availTime2.Content :
                        (l == this.DoctorLeeTile.availTime1) ? (String)this.DoctorLeeTile.availTime1.Content :
                        (l == this.DoctorLeeTile.availTime2) ? (String)this.DoctorLeeTile.availTime2.Content :
                        (l == this.DoctorWalterTile.availTime1) ? (String)this.DoctorWalterTile.availTime1.Content :
                        (l == this.DoctorWalterTile.availTime2) ? (String)this.DoctorWalterTile.availTime2.Content : null;

            if (s != null)
            {
                try
                {
                    DateTime dt = DateTime.Parse(s);

                    m_dayViewControl = new DayViewControl(dt, m_appointmentDatabase);
                    MainGrid.Children.Add(m_dayViewControl);
                    Grid.SetRow(m_dayViewControl, 0);
                    Grid.SetColumn(m_dayViewControl, 1);
                    Grid.SetRowSpan(m_dayViewControl, 3);
                    m_dayViewControl.Visibility = Visibility.Visible;
                }
                catch (Exception ex) { /* probably because available time can't be parsed, just do nothing*/ }
            }
        }
    }
}
