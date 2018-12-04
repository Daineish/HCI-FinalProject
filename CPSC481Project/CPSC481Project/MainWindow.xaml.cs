﻿using System;
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



namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        String[] names = { "Jason","Marge", "Eddie", "Nicky", "Alex", "Jason", "Jessy", "Sally", "Misty", "Sam", "Ron", "Ted", "Jenny", "Nancy", "Sophia", "Steve", "Adam", "Kyle", "Kevin", "Ricky", "Eunice", "Jane", "Mike", "Noah", "William", "David", "Mary", "Patricia", "Linda", "Barbara", "Susan", "Elizabeth" };
        String[] lastnames = { "Adam", "Johnson", "Williamson", "Smith", "Wong", "Nguyen", "Tran", "Lee", "Isaac", "Liamson", "Ronald", "Donald", "McDonald", "Davidson", "Larson" };
        String[] doctors = { "Dr.Lee", "Dr.Walter", "Dr.Payne" };
        String[] domain = { "@hotmail.com", "@gmail.com", "@gmail.ca", "@yahoo.ca", "@yahoo.com", "@shaw.ca", "@gmail.ca", "@live.ca", "@live.com" };
        String[] addr = { "University", "Edgemont", "Hamptons", "Hawkwood", "Citadel", "Sandstone", "Country", "Ave", "Downtown" };
        String[] addr2 = { "Street", "Drive", "Memorial", "Dale", "Rise", "Mews", "Place", "Bank" };
        String[] month = { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        internal DateTime _DisplayStartDate = DateTime.Now;

        Patient m_currentPatient;
        Boolean selectedMode;
        PatientDatabase m_patientDatabase;
		AppointmentDatabase m_appointmentDatabase;
        public MainWindow()
        {
            setTimer();
            InitializeComponent();
            m_patientDatabase = new PatientDatabase();
			m_appointmentDatabase = new AppointmentDatabase();

            //get current date for dashboard
            DashDate.Content = month.ElementAt(_DisplayStartDate.Month - 1) + " " + _DisplayStartDate.Day + ", " + _DisplayStartDate.Year;

if (m_patientDatabase.NumPatients() < 10)
            {
                for (int i = 0; i < 90; i++)
                {
                    String firstName = names[rnd.Next(0, names.Length)];
                    String lastName = lastnames[rnd.Next(0, lastnames.Length)];
                    String hcNum = "0000" + i;
                    String email = firstName.ToLower() + "." + lastName.ToLower() + domain[rnd.Next(0, domain.Length)];
                    String address = rnd.Next(100, 999) + " " + addr[rnd.Next(0, addr.Length)] + " " + addr2[rnd.Next(0, addr2.Length)];
                    String phone = "(" + rnd.Next(100, 999) + ") - " + rnd.Next(100, 999) + " - " + rnd.Next(1000, 9999);
                    Patient p1 = new Patient(lastName, firstName, hcNum, address, email, phone);

                    m_patientDatabase.AddPatient(p1);
                }
            }

            if (m_appointmentDatabase.NumAppointments() < 5)
            {
                Patient p = new Patient("Appointments", "Mister", "99876", "Not Available", "unknown", "unknown");
                m_patientDatabase.AddPatient(p);
                for (int i = 0; i < 10; i++)
                {
                    Appointment a = new Appointment(p, "Dr. Walter", DateTime.Today, DateTime.Today, "Appointment #" + i);
                    m_appointmentDatabase.AddAppointment(a);
                }
            }

            // Debug - show all patients and appointments in console.
            m_appointmentDatabase.PrintAllAppointments();
            m_patientDatabase.PrintAllPatients();
            randomPatientGenerator();

            
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

            this.Dispatcher.Invoke(() =>
            {
                this.DashTime.Content = hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + apm;
                this.DashDate.Content = month.ElementAt(DateTime.Now.Month - 1) + " " + DateTime.Now.Day + ", " + DateTime.Now.Year;
            });
            
        }

        public void randomPatientGenerator()
        {
            
            //Populating Past Patients and Randomly Selecting next  Patients
            Patient m = m_patientDatabase.findPatient("0000"+rnd.Next(10,20).ToString());
            Patient j = m_patientDatabase.findPatient("0000" + rnd.Next(20, 30).ToString());
            Patient d = m_patientDatabase.findPatient("0000" + rnd.Next(30, 40).ToString());
            Patient l = m_patientDatabase.findPatient("0000" + rnd.Next(70, 80).ToString());
            Patient k = m_patientDatabase.findPatient("0000" + rnd.Next(80, 90).ToString());


            //Patient LastName First Name
            pnameLbl.Content = m.GetLastName() + ", " + m.GetFirstName();
            pnameLbl1.Content = j.GetLastName() + ", " +  j.GetFirstName();
            pnameLbl2.Content = d.GetLastName() + ", " +  d.GetFirstName();
            pnameLbl3.Content = l.GetLastName() + ", " + l.GetFirstName();
            pnameLbl4.Content = k.GetLastName() + ", " + k.GetFirstName();
            //Patient HC#
            phcLbl.Content = m.GetHCNumber();
            phcLbl1.Content = j.GetHCNumber();
            phcLbl2.Content = d.GetHCNumber();
            phcLbl3.Content = l.GetHCNumber();
            phcLbl4.Content = k.GetHCNumber();
            //PatientPhone
            pnumLbl.Content = m.GetPhone();
            pnumLbl1.Content = j.GetPhone();
            pnumLbl2.Content = d.GetPhone();
            pnumLbl3.Content = l.GetPhone();
            pnumLbl4.Content = k.GetPhone();
            //Patient Previous Doctor
            pprevLbl.Content = doctors[rnd.Next(0, 3)];
            pprevLbl1.Content = doctors[rnd.Next(0, 3)];
            pprevLbl2.Content = doctors[rnd.Next(0, 3)];
            pprevLbl3.Content = doctors[rnd.Next(0, 3)];
            pprevLbl4.Content = doctors[rnd.Next(0, 3)];
            //Patient Address
            paddrLbl.Content = m.GetAddress();
            paddrLbl1.Content = j.GetAddress();
            paddrLbl2.Content = d.GetAddress();
            paddrLbl3.Content = l.GetAddress();
            paddrLbl4.Content = k.GetAddress();
            //Patient Email
            pemailLbl.Content = m.GetEmail();
            pemailLbl1.Content = j.GetEmail();
            pemailLbl2.Content = d.GetEmail();
            pemailLbl3.Content = l.GetEmail();
            pemailLbl4.Content = k.GetEmail();

            Patient a = m_patientDatabase.findPatient("0000" + rnd.Next(10, 20).ToString());
            Patient b = m_patientDatabase.findPatient("0000" + rnd.Next(20, 30).ToString());
            Patient c = m_patientDatabase.findPatient("0000" + rnd.Next(30, 40).ToString());


            Patient z = m_patientDatabase.findPatient("0000" + rnd.Next(40, 50).ToString());
            Patient x = m_patientDatabase.findPatient("0000" + rnd.Next(50, 60).ToString());
            Patient y = m_patientDatabase.findPatient("0000" + rnd.Next(60, 70).ToString());

            this.DoctorWalterTile.npfullName2.Content = c.GetLastName() + ", " + c.GetFirstName() + " HC: " + c.GetHCNumber();
            this.DoctorLeeTile.npfullName2.Content = b.GetLastName() + ", " + b.GetFirstName() + " HC: " + b.GetHCNumber();
            this.DoctorPayneTile.npfullName2.Content = a.GetLastName() + ", " + a.GetFirstName() + " HC: " + a.GetHCNumber();
            this.DoctorWalterTile.npfullName.Content = z.GetLastName() + ", " + z.GetFirstName() + " HC: " + z.GetHCNumber();
            this.DoctorLeeTile.npfullName.Content = x.GetLastName() + ", " +x.GetFirstName() + " HC: " + x.GetHCNumber();
            this.DoctorPayneTile.npfullName.Content = y.GetLastName() + ", " + y.GetFirstName() + " HC: " + y.GetHCNumber();

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
            //This checks to see if our searchField isnt empty and if information is inputted correctly.
            if(searchField.Text != "Search (Healthcare #, Name)" & searchField.Text != "")
            {
                //Test example
                //patientLabel.Content = "Hi";

                if (IsDigitsOnly(searchField.Text))
                {
                    List<Patient> patients = m_patientDatabase.FindPatientHC(searchField.Text);
                    double size = 0;
                    foreach (Patient pat in patients)
                    {
                        Grid g = CreateGrid(pat);
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
                        PatientListStackPanel.Children.Add(CreateGrid(pat));
                    }

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
            //rpatientBox.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            if (sender == recentPgrid)
            {
                rpatientBox.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }
            if (sender == recentPgrid2)
            {
                rpatientBox2.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }
            if (sender == recentPgrid3)
            {
                rpatientBox3.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }
            if (sender == recentPgrid4)
            {
                rpatientBox4.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }
            if (sender == recentPgrid5)
            {
                rpatientBox5.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            }
        }

        private void rpatientExit(object sender, MouseEventArgs e)
        {
            if(sender == recentPgrid)
            {
                rpatientBox.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            if(sender == recentPgrid2)
            {
                rpatientBox2.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            if (sender == recentPgrid3)
            {
                rpatientBox3.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            if (sender == recentPgrid4)
            {
                rpatientBox4.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            if (sender == recentPgrid5)
            {
                rpatientBox5.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }

        }

        private void searchField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /**
         * When searching for a patient, this method can be called to create a grid showing a patient's information.
         */
        private Grid CreateGrid(Patient p, Boolean isSearch = true)
        {
            Grid g = new Grid();
            Label nameL  = new Label();
            Label hcL    = new Label();
            Label addyL  = new Label();
            Label emailL = new Label();
            Label phoneL = new Label();

            nameL.Content =  "Name:";    nameL.Height  = 40; nameL.Margin  = new Thickness(16,  5,0,0); nameL.Width  = 101; nameL.FontSize  = 18; nameL.HorizontalAlignment  = HorizontalAlignment.Left; nameL.VerticalAlignment  = VerticalAlignment.Top;
            hcL.Content =    "HC#:";     hcL.Height    = 40; hcL.Margin    = new Thickness(16, 45,0,0); hcL.Width    = 101; hcL.FontSize    = 18; hcL.HorizontalAlignment    = HorizontalAlignment.Left; hcL.VerticalAlignment    = VerticalAlignment.Top;
            addyL.Content =  "Address:"; addyL.Height  = 40; addyL.Margin  = new Thickness(16, 85,0,0); addyL.Width  = 101; addyL.FontSize  = 18; addyL.HorizontalAlignment  = HorizontalAlignment.Left; addyL.VerticalAlignment  = VerticalAlignment.Top;
            emailL.Content = "Email:";   emailL.Height = 40; emailL.Margin = new Thickness(16,125,0,0); emailL.Width = 101; emailL.FontSize = 18; emailL.HorizontalAlignment = HorizontalAlignment.Left; emailL.VerticalAlignment = VerticalAlignment.Top;
            phoneL.Content = "Phone:";   phoneL.Height = 40; phoneL.Margin = new Thickness(16,165,0,0); phoneL.Width = 101; phoneL.FontSize = 18; phoneL.HorizontalAlignment = HorizontalAlignment.Left; phoneL.VerticalAlignment = VerticalAlignment.Top;

            Label nameC  = new Label();
            Label hcC    = new Label();
            Label addyC  = new Label();
            Label emailC = new Label();
            Label phoneC = new Label();
           
            nameC.Content  = p.GetLastName() + ", " + p.GetFirstName(); nameC.Height  = 40; nameC.Margin  = new Thickness(122,  5,0,0); nameC.Width  = 300; nameC.FontSize  = 18; nameC.FontWeight  = FontWeights.Bold; nameC.HorizontalAlignment  = HorizontalAlignment.Left; nameC.VerticalAlignment  = VerticalAlignment.Top;
            hcC.Content    = p.GetHCNumber();  hcC.Height    = 40; hcC.Margin    = new Thickness(122, 45,0,0); hcC.Width    = 300; hcC.FontSize    = 18; hcC.FontWeight    = FontWeights.Bold; hcC.HorizontalAlignment    = HorizontalAlignment.Left; hcC.VerticalAlignment    = VerticalAlignment.Top;
            addyC.Content  = p.GetAddress();   addyC.Height  = 40; addyC.Margin  = new Thickness(122, 85,0,0); addyC.Width  = 300; addyC.FontSize  = 18; addyC.FontWeight  = FontWeights.Bold; addyC.HorizontalAlignment  = HorizontalAlignment.Left; addyC.VerticalAlignment  = VerticalAlignment.Top;
            emailC.Content = p.GetEmail();     emailC.Height = 40; emailC.Margin = new Thickness(122,125,0,0); emailC.Width = 300; emailC.FontSize = 18; emailC.FontWeight = FontWeights.Bold; emailC.HorizontalAlignment = HorizontalAlignment.Left; emailC.VerticalAlignment = VerticalAlignment.Top;
            phoneC.Content = p.GetPhone();     phoneC.Height = 40; phoneC.Margin = new Thickness(122,165,0,0); phoneC.Width = 300; phoneC.FontSize = 18; phoneC.FontWeight = FontWeights.Bold; phoneC.HorizontalAlignment = HorizontalAlignment.Left; phoneC.VerticalAlignment = VerticalAlignment.Top;

            Rectangle r = new Rectangle();
            r.Stroke = System.Windows.Media.Brushes.Black;
            r.HorizontalAlignment = HorizontalAlignment.Left;
            r.VerticalAlignment = VerticalAlignment.Top;
            r.Width = 422;
            r.Height = 245;

            if(isSearch)
            {
                Button b = new Button();
                b.Content = "Select";
                b.Width = 100;
                b.Height = 30;
                b.Margin = new Thickness(150, 200, 0, 0);
                b.Tag = p;
                b.Click += SelectPatientClicked;
                g.Children.Add(b);
            }
            else
                r.Fill = new SolidColorBrush(Color.FromRgb(249, 209, 26));
            
            g.Children.Add(r);
            g.Children.Add(nameL); g.Children.Add(hcL); g.Children.Add(addyL); g.Children.Add(emailL); g.Children.Add(phoneL);
            g.Children.Add(nameC); g.Children.Add(hcC); g.Children.Add(addyC); g.Children.Add(emailC); g.Children.Add(phoneC);
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
            PatientListStackPanel.Children.Add(CreateGrid(m_currentPatient, false));

            selectedMode = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //dashboard.Visibility = Visibility.Hidden;
            dayview.Visibility = Visibility.Visible;
        }
        /*private void Back_Click(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Visible;
            dayview.Visibility = Visibility.Hidden;
        }*/

        private void calendarBack(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Visible;
            calendarview.Visibility = Visibility.Hidden;
        }
        private void ToCalendar_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            //dashboard.Visibility = Visibility.Hidden;
            calendarview.Visibility = Visibility.Visible;
        }

        private void ToDayView_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            //dashboard.Visibility = Visibility.Hidden;
            dayview.Visibility = Visibility.Visible;
        }

        //Popup
        private void addContact(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Add Contact", "Add Contact");
            recentLabel.Content = "Add Patient:";
            PatientListScrollViewer.Visibility = Visibility.Hidden;
            addPatient.Visibility = Visibility.Visible;
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


    }
}
