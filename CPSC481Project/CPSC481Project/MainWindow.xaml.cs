using System;
using System.Collections.Generic;
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
        VacationDatabase m_vacationDatabase;
        DayViewControl m_dayViewControl;
        MonthlyViewControl m_monthlyViewControl;

        private int queuePosition = 0;

        public MainWindow()
        {
            setTimer();
            InitializeComponent();
            m_patientDatabase = new PatientDatabase();
            m_appointmentDatabase = new AppointmentDatabase();
            m_vacationDatabase = new VacationDatabase();

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
                TimeSpan ts = new TimeSpan(10, 0, 0);
                today = today.Date + ts;
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00001"), "Dr. Walter", today, today.AddMinutes(10), "Appointment #1"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("12345"), "Dr. Payne", today.AddMinutes(30), today.AddMinutes(40), "Appointment #1"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00002"), "Dr. Lee", today.AddDays(1), today.AddDays(1).AddMinutes(10), "Appointment #1"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("44444"), "Dr. Walter", today.AddHours(4), today.AddHours(4).AddMinutes(10), "Appointment #1"));
                m_appointmentDatabase.AddAppointment(new Appointment(m_patientDatabase.findPatient("00001"), "Dr. Walter", today.AddMinutes(-20), today.AddMinutes(-10), "Appointment #1"));
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
                    this.DoctorPayneTile.npfullName.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                if(i == 1 && pat != null)
                    this.DoctorPayneTile.npfullName2.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
            }
            for (int i = 0; i < 2 && i < appointmentLee1.Count(); i++)
            {
                Appointment app = appointmentLee1.ElementAt(i);
                Patient pat = appointmentLee1.ElementAt(i).m_patient;
                if (i == 0 && pat != null)
                    this.DoctorLeeTile.npfullName.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                if (i == 1 && pat != null)
                    this.DoctorLeeTile.npfullName2.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
            }
            for (int i = 0; i < 2 && i < appointmentWalter1.Count(); i++)
            {
                Appointment app = appointmentWalter1.ElementAt(i);
                Patient pat = appointmentWalter1.ElementAt(i).m_patient;
                if(i == 0 && pat != null)
                    this.DoctorWalterTile.npfullName.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
                if (i == 1 && pat != null)
                    this.DoctorWalterTile.npfullName2.Content = app.m_startTime.ToString("hh:mm") + ": " + pat.GetLastName() + ", " + pat.GetFirstName();
            }

            // Populate recent patient list with fake data (TODO)
            Patient recent1 = m_patientDatabase.findPatient("15432");
            Patient recent2 = m_patientDatabase.findPatient("15795");
            Patient recent3 = m_patientDatabase.findPatient("11325");
            Patient recent4 = m_patientDatabase.findPatient("99999");
            Patient recent5 = m_patientDatabase.findPatient("83409");

            PatientListStackPanel.Children.Add(CreateGrid(recent1));
            PatientListStackPanel.Children.Add(CreateGrid(recent2));
            PatientListStackPanel.Children.Add(CreateGrid(recent3));
            PatientListStackPanel.Children.Add(CreateGrid(recent4));
            PatientListStackPanel.Children.Add(CreateGrid(recent5));

            // Populate available times
            List<String> availablePayne = m_appointmentDatabase.AvailableTimes("Dr. Payne");
            List<String> availableLee = m_appointmentDatabase.AvailableTimes("Dr. Lee");
            List<String> availableWalter = m_appointmentDatabase.AvailableTimes("Dr. Walter");
            for (int i = 0; i < 2 && i < availablePayne.Count(); i++)
            {
                String str = availablePayne.ElementAt(i);
                if (i == 0 && str != null)
                    this.DoctorPayneTile.availTime1.Content = str;
                else if (i == 1 && str != null)
                    this.DoctorPayneTile.availTime2.Content = str;
            }
            for (int i = 0; i < 2 && i < availableLee.Count(); i++)
            {
                String str = availableLee.ElementAt(i);
                if (i == 0 && str != null)
                    this.DoctorLeeTile.availTime1.Content = str;
                else if (i == 1 && str != null)
                    this.DoctorLeeTile.availTime2.Content = str;
            }
            for (int i = 0; i < 2 && i < availableWalter.Count(); i++)
            {
                String str = availableWalter.ElementAt(i);
                if (i == 0 && str != null)
                    this.DoctorWalterTile.availTime1.Content = str;
                else if (i == 1 && str != null)
                    this.DoctorWalterTile.availTime2.Content = str;
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
            Console.WriteLine("Search has been clicked\n");
            searchField.Text = "";
            searchField.Opacity = 100;
        }



        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // first clear StackPanel contents
            PatientListStackPanel.Children.Clear();
            recentLabel.Content = "Patient Search";

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

        }

        /**
         * When searching for a patient, this method can be called to create a grid showing a patient's information.
         */
        private Grid CreateGrid(Patient p, Boolean withButton = false, Boolean highlighted = false)
        {
            Grid g = new Grid();
            Label nameL = new Label();
            Label hcL = new Label();
            Label addyL = new Label();
            Label emailL = new Label();
            Label phoneL = new Label();

            nameL.Content = "Name:"; nameL.Height = 40; nameL.Margin = new Thickness(16, 5, 0, 0); nameL.Width = 101; nameL.FontSize = 18; nameL.HorizontalAlignment = HorizontalAlignment.Left; nameL.VerticalAlignment = VerticalAlignment.Top;
            hcL.Content = "HC#:"; hcL.Height = 40; hcL.Margin = new Thickness(16, 45, 0, 0); hcL.Width = 101; hcL.FontSize = 18; hcL.HorizontalAlignment = HorizontalAlignment.Left; hcL.VerticalAlignment = VerticalAlignment.Top;
            addyL.Content = "Address:"; addyL.Height = 40; addyL.Margin = new Thickness(16, 85, 0, 0); addyL.Width = 101; addyL.FontSize = 18; addyL.HorizontalAlignment = HorizontalAlignment.Left; addyL.VerticalAlignment = VerticalAlignment.Top;
            emailL.Content = "Email:"; emailL.Height = 40; emailL.Margin = new Thickness(16, 125, 0, 0); emailL.Width = 101; emailL.FontSize = 18; emailL.HorizontalAlignment = HorizontalAlignment.Left; emailL.VerticalAlignment = VerticalAlignment.Top;
            phoneL.Content = "Phone:"; phoneL.Height = 40; phoneL.Margin = new Thickness(16, 165, 0, 0); phoneL.Width = 101; phoneL.FontSize = 18; phoneL.HorizontalAlignment = HorizontalAlignment.Left; phoneL.VerticalAlignment = VerticalAlignment.Top;

            TextBlock nameC = new TextBlock();
            TextBlock hcC = new TextBlock();
            TextBlock addyC = new TextBlock();
            TextBlock emailC = new TextBlock();
            TextBlock phoneC = new TextBlock();

            nameC.Text = p.GetLastName() + ", " + p.GetFirstName(); nameC.Height = 40; nameC.Margin = new Thickness(100, 5, 0, 0); nameC.Width = 230; nameC.FontSize = 18; nameC.FontWeight = FontWeights.Bold; nameC.HorizontalAlignment = HorizontalAlignment.Left; nameC.VerticalAlignment = VerticalAlignment.Top; nameC.TextTrimming = TextTrimming.CharacterEllipsis;
            hcC.Text = p.GetHCNumber(); hcC.Height = 40; hcC.Margin = new Thickness(100, 45, 0, 0); hcC.Width = 230; hcC.FontSize = 18; hcC.FontWeight = FontWeights.Bold; hcC.HorizontalAlignment = HorizontalAlignment.Left; hcC.VerticalAlignment = VerticalAlignment.Top; hcC.TextTrimming = TextTrimming.CharacterEllipsis;
            addyC.Text = p.GetAddress(); addyC.Height = 40; addyC.Margin = new Thickness(100, 85, 0, 0); addyC.Width = 230; addyC.FontSize = 18; addyC.FontWeight = FontWeights.Bold; addyC.HorizontalAlignment = HorizontalAlignment.Left; addyC.VerticalAlignment = VerticalAlignment.Top; addyC.TextTrimming = TextTrimming.CharacterEllipsis;
            emailC.Text = p.GetEmail(); emailC.Height = 40; emailC.Margin = new Thickness(100, 125, 0, 0); emailC.Width = 230; emailC.FontSize = 18; emailC.FontWeight = FontWeights.Bold; emailC.HorizontalAlignment = HorizontalAlignment.Left; emailC.VerticalAlignment = VerticalAlignment.Top; emailC.TextTrimming = TextTrimming.CharacterEllipsis;
            phoneC.Text = p.GetPhone(); phoneC.Height = 40; phoneC.Margin = new Thickness(100, 165, 0, 0); phoneC.Width = 230; phoneC.FontSize = 18; phoneC.FontWeight = FontWeights.Bold; phoneC.HorizontalAlignment = HorizontalAlignment.Left; phoneC.VerticalAlignment = VerticalAlignment.Top; phoneC.TextTrimming = TextTrimming.CharacterEllipsis;

            Rectangle r = new Rectangle();
            r.Stroke = System.Windows.Media.Brushes.Black;
            r.HorizontalAlignment = HorizontalAlignment.Left;
            r.VerticalAlignment = VerticalAlignment.Top;
            r.Width = 422;
            r.Height = 245;

            if (withButton)
            {
                Button b = new Button();
                b.Content = "Select";
                b.Width = 100;
                b.Height = 30;
                b.Margin = new Thickness(-50, 200, 0, 0);
                b.Tag = p;
                b.Cursor = Cursors.Hand;
                b.Click += SelectPatientClicked;
                g.Children.Add(b);
            }
            else if(highlighted)
            {
                r.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }

            //Add button to view patient information
            Button view = new Button();
            view.Content = "View";
            view.Width = 75;
            view.Height = 30;
            view.Margin = new Thickness(150, 200, 0, 0);
            view.Tag = p;
            view.Cursor = Cursors.Hand;
            view.Click += selectViewPatient;
            g.Children.Add(view);

            g.Children.Add(r);
            g.Children.Add(nameL); g.Children.Add(hcL); g.Children.Add(addyL); g.Children.Add(emailL); g.Children.Add(phoneL);
            g.Children.Add(nameC); g.Children.Add(hcC); g.Children.Add(addyC); g.Children.Add(emailC); g.Children.Add(phoneC);

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
            return g;
        }

        /**
         * The handler of when the "select patient" button is clicked.
         */
        private void SelectPatientClicked(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            // Remove all boxes, then add back the selected patient box.
            PatientListStackPanel.Children.Clear();
            m_currentPatient = (Patient)b.Tag;
            PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, false, true));

            selectedMode = true;
        }

        private void unSelectPatientClicked(object sender, RoutedEventArgs e)
        {
            Button unselectPatient = (Button)sender;

            // Remove all boxes, then add back the selected patient box.
            PatientListStackPanel.Children.Clear();
            m_currentPatient = (Patient)unselectPatient.Tag;
            PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, true, false));

            selectedMode = false;
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

            //Also, need to fix the "recent" label and add an exit button to exit out of viewing patient info
            recentLabel.Content = "View Patient:";
            //Reuse the same button as for exiting search
            RemoveSearchButton.Visibility = Visibility.Visible;

            //Future work: Add an edit button to edit any field
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

            //Keep text the same, let users change if they want
            dPatientnameBox.Text = dPatientname.Text;
            dPatientlnameBox.Text = dPatientlname.Text;
            dPatientHCBox.Text = dPatientHC.Text;
            dPatientaddrBox.Text = dPatientaddr.Text;
            dPatientpnBox.Text = dPatientpn.Text;
            dPatientemailBox.Text = dPatientemail.Text;

            //Then show the text boxes
            dPatientnameBox.Visibility = Visibility.Visible;
            dPatientlnameBox.Visibility = Visibility.Visible;
            dPatientHCBox.Visibility = Visibility.Visible;
            dPatientaddrBox.Visibility = Visibility.Visible;
            dPatientpnBox.Visibility = Visibility.Visible;
            dPatientemailBox.Visibility = Visibility.Visible;

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

            //Now hide the text boxes and make text fields visible
            dPatientnameBox.Visibility = Visibility.Hidden;
            dPatientlnameBox.Visibility = Visibility.Hidden;
            dPatientHCBox.Visibility = Visibility.Hidden;
            dPatientaddrBox.Visibility = Visibility.Hidden;
            dPatientpnBox.Visibility = Visibility.Hidden;
            dPatientemailBox.Visibility = Visibility.Hidden;

            dPatientname.Visibility = Visibility.Visible;
            dPatientlname.Visibility = Visibility.Visible;
            dPatientHC.Visibility = Visibility.Visible;
            dPatientaddr.Visibility = Visibility.Visible;
            dPatientpn.Visibility = Visibility.Visible;
            dPatientemail.Visibility = Visibility.Visible;
            
            //Replace and edit information.
            Patient p = new Patient(dPatientlnameBox.Text, dPatientnameBox.Text, dPatientHCBox.Text, dPatientaddrBox.Text, dPatientemailBox.Text, dPatientpnBox.Text);
            if(m_patientDatabase.findPatient(p.m_hcNumber)!= null)
            {
                m_patientDatabase.RemovePatient(p.m_hcNumber);
                m_patientDatabase.AddPatient(p);
            }


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

            //Then make text fields visible
            dPatientname.Visibility = Visibility.Visible;
            dPatientlname.Visibility = Visibility.Visible;
            dPatientHC.Visibility = Visibility.Visible;
            dPatientaddr.Visibility = Visibility.Visible;
            dPatientpn.Visibility = Visibility.Visible;
            dPatientemail.Visibility = Visibility.Visible;

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
            if(filterMode == false || filterDoctor.Visibility == Visibility.Hidden)
            {
                filterMode = true;
                PatientListScrollViewer.Height = 708.5 - 180.5;
                filterDoctor.Visibility = Visibility.Visible;
            }


            //dashboard.Visibility = Visibility.Hidden;
            m_monthlyViewControl = new MonthlyViewControl();
            m_monthlyViewControl.Visibility = Visibility.Visible;
            MainGrid.Children.Add(m_monthlyViewControl);
            Grid.SetRow(m_monthlyViewControl, 0);
            Grid.SetColumn(m_monthlyViewControl, 1);
            Grid.SetRowSpan(m_monthlyViewControl, 3);
        }

        public void ToVacayCalendar(string drName)
        {
            //somehow receive doctor name and use that to control filter for calendar?
            m_monthlyViewControl = new MonthlyViewControl();
            m_monthlyViewControl.Visibility = Visibility.Visible;
            MainGrid.Children.Add(m_monthlyViewControl);
            Grid.SetRow(m_monthlyViewControl, 0);
            Grid.SetColumn(m_monthlyViewControl, 1);
            Grid.SetRowSpan(m_monthlyViewControl, 3);
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
            recentLabel.Content = "Add Patient:";
            PatientListScrollViewer.Visibility = Visibility.Hidden;
            addPatient.Visibility = Visibility.Visible;
            clearAddtext();
            InitAddPatientTextfields();
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

            if (newName != "" && newLname != "" && newHC != "" && newPhone != "" && email != "" && addr != "")
            {
                PatientListScrollViewer.Visibility = Visibility.Hidden;
                confirmBtn.Visibility = Visibility.Hidden;
                Yes.Visibility = Visibility.Visible;
                warning.Visibility = Visibility.Visible;
                warning.Text = "Are you sure?";

            }
            //Add error check for names (letters only)
            else if (!IsLettersOnly(newName) || !IsLettersOnly(newLname))
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
            else if (!IsDigitsOnly(newPhone))
            {
                warning.Text = "Please ensure that the phone number only contains digits";
                warning.Visibility = Visibility.Visible;
            }
            else if (newName == "" || newLname == "" || newHC == "" || newPhone == "" || email == "" || addr == "")
            {
                warning.Text = "Please ensure all required fields are completed.";
                warning.Visibility = Visibility.Visible;
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
        }

        public AppointmentDatabase NewAppointmentClicked(DateTime datetime, String doc)
        {
            if (m_currentPatient == null)
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // TODO: Dialog box to enter additional appointment information or something like in prototypes?
                String infoString = "Confirm appointment on " + datetime.ToString("dddd MMMM d, yyyy") + " at " + datetime.ToString("t") + " with " + doc + "?";
                if (MessageBox.Show(infoString, "Confirm New Appointment", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    m_appointmentDatabase.AddAppointment(new Appointment(m_currentPatient, doc, datetime, datetime.AddMinutes(10), ""));
                    PopulateDefaultInfo();
                    MessageBox.Show("Appointment added", "Appointment added", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
                else
                {
                    // Do nothing
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
                queuePosition++;
                WalkinTile t = new WalkinTile(m_currentPatient.m_firstName + " " + m_currentPatient.m_lastName, m_currentPatient.m_hcNumber, queuePosition);
                Console.WriteLine("Hi");
                //Console.WriteLine(t.getHC());
                string text = "";
                foreach(var item in walkinQueueList.Items)
                {
                    Console.WriteLine(item);

                }

                Console.WriteLine(text);


                walkinQueueList.Items.Add(t);
            }
        }

        public void MonthViewToDayView(DateTime d)
        {
            if (filterMode == true)
            {
                filterDoctor.Visibility = Visibility.Hidden;
                PatientListScrollViewer.Height = 708.5;
                Console.WriteLine(PatientListScrollViewer.Height);
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
		
		//Initialize fields when opening the add patient window so users get an example
        private void InitAddPatientTextfields()
        {
            apnameField.Text = "Jane";
            aplnameField.Text = "Smith";
            aphcField.Text = "00000";
            apaddrField.Text = "12 University Dr";
            apphoneField.Text = "(403) 123-4567";
            apemailField.Text = "abc@abc.com";
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
            RemoveSearchButton.Visibility = Visibility.Hidden;

            //If this is remove for the viewPatient panel
            if (viewPatient.Visibility == Visibility.Visible)
            {
                viewPatient.Visibility = Visibility.Hidden;
            }
        }
        //Filter Checked
        private void payneChecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Payne Checked");
        }

        private void leeChecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Lee Checked");
        }

        private void walterChecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Walter Checked");
        }

        private void payneUnchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Payne Unchecked");
        }

        private void leeUnchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Lee Unchecked");
        }

        private void walterUnchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Walter Unchecked");
        }
        //Filter Unchecked
    }
}
