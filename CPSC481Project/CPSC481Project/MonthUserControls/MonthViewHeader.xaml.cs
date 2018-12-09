using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Globalization;

namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for MonthViewHeader.xaml
    /// </summary>
    public partial class MonthViewHeader : UserControl
    {
        String[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
        internal DateTime _DisplayStartDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));
        //internal DateTime _DisplayStartDate = DateTime.Now;
        private int _DisplayMonth = new int();
        private int _DisplayYear = new int();
        private CultureInfo _cultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.LCID);
        private System.Globalization.Calendar sysCal;
        private List<Vacation> _monthAppointments = new List<Vacation>();

        public event DisplayMonthChangedEventHandler DisplayMonthChanged;

        public delegate void DisplayMonthChangedEventHandler(MonthChangedEventArgs e);

        public event DayBoxDoubleClickedEventHandler DayBoxDoubleClicked;

        public delegate void DayBoxDoubleClickedEventHandler(NewAppointmentEventArgs e);

        public event AppointmentDblClickedEventHandler AppointmentDblClicked;

        public delegate void AppointmentDblClickedEventHandler(String doc, DateTime st, DateTime end);

        

        public DateTime DisplayStartDate
        {
            get
            {
                return _DisplayStartDate;
            }
            set
            {
                _DisplayStartDate = value;
                _DisplayMonth = _DisplayStartDate.Month;
                _DisplayYear = _DisplayStartDate.Year;
            }
        }

        internal List<Vacation> MonthAppointments
        {
            get
            {
                return _monthAppointments;
            }
            set
            {
                _monthAppointments = value;
                BuildCalendarUI();
            }
        }

        public MonthViewHeader()
        {
            InitializeComponent();
            // -- Want to have the calendar show up, even if no appoints are assigned 
            // Note - in my own app, appointments are loaded by a backgroundWorker thread to avoid a laggy UI
            _DisplayMonth = _DisplayStartDate.Month;
            _DisplayYear = _DisplayStartDate.Year;
            sysCal = _cultureInfo.Calendar;
            if (_monthAppointments == null)
                BuildCalendarUI();
        }

        private void BuildCalendarUI()
        {
            int iDaysInMonth = sysCal.GetDaysInMonth(_DisplayStartDate.Year, _DisplayStartDate.Month);
            int iOffsetDays = System.Convert.ToInt32(System.Enum.ToObject(typeof(System.DayOfWeek), _DisplayStartDate.DayOfWeek));
            int iWeekCount = 0;
            WeekOfDaysControls weekRowCtrl = new WeekOfDaysControls();

            MonthViewGrid.Children.Clear();
            AddRowsToMonthGrid(iDaysInMonth, iOffsetDays);
            MonthYearLabel.Content = months.ElementAt(_DisplayMonth - 1) + " " + _DisplayYear;

            for (int i = 1; i <= iDaysInMonth; i++)
            {
                if ((i != 1) && System.Math.IEEERemainder((i + iOffsetDays - 1), 7) == 0)
                {
                    // -- add existing weekrowcontrol to the monthgrid
                    Grid.SetRow(weekRowCtrl, iWeekCount);
                    MonthViewGrid.Children.Add(weekRowCtrl);
                    // -- make a new weekrowcontrol
                    weekRowCtrl = new WeekOfDaysControls();
                    iWeekCount += 1;
                }

                // -- load each weekrow with a DayBoxControl whose label is set to day number
                DayBoxControl dayBox = new DayBoxControl(i);
                dayBox.DayNumberLabel.Content = i.ToString();
                dayBox.Tag = i;
                dayBox.MouseDoubleClick += DayBox_DoubleClick;
                dayBox.DragEnter += DayBox_DragEnter;

                // -- customize daybox for today:
                if ((new DateTime(_DisplayYear, _DisplayMonth, i)) == DateTime.Today)
                {
                    dayBox.DayLabelRowBorder.Background = (Brush)dayBox.TryFindResource("OrangeGradientBrush");
                    dayBox.DayAppointmentsStack.Background = Brushes.Wheat;
                }

                if (_monthAppointments != null)
                {
                    int iday = i;
                    Predicate<Vacation> aptFind = delegate(Vacation apt) { return (int)apt.m_curDate.Day == iday; };
                    List<Vacation> aptInDay = _monthAppointments.FindAll(aptFind);
                    foreach (Vacation a in aptInDay)
                    {
                        DayBoxAppointmentControl apt = new DayBoxAppointmentControl();
                        apt.SetDoctor(a.m_doctor);
                        apt.m_startDate = a.m_startDate;
                        apt.m_endDate = a.m_endDate;
                        apt.DisplayText.Text = a.m_doctor;
                        apt.MouseDoubleClick += Appointment_DoubleClick;
                        dayBox.DayAppointmentsStack.Children.Add(apt);
                    }
                }

                Grid.SetColumn(dayBox, (i - (iWeekCount * 7)) + iOffsetDays);
                weekRowCtrl.WeekRowGrid.Children.Add(dayBox);
            }
            Grid.SetRow(weekRowCtrl, iWeekCount);
            MonthViewGrid.Children.Add(weekRowCtrl);
        }

        private void AddRowsToMonthGrid(int DaysInMonth, int OffSetDays)
        {
            MonthViewGrid.RowDefinitions.Clear();
            System.Windows.GridLength rowHeight = new System.Windows.GridLength(60, System.Windows.GridUnitType.Star);

            int EndOffSetDays = 7 - (System.Convert.ToInt32(System.Enum.ToObject(typeof(System.DayOfWeek), _DisplayStartDate.AddDays(DaysInMonth - 1).DayOfWeek)) + 1);

            for (int i = 1; i <= System.Convert.ToInt32((DaysInMonth + OffSetDays + EndOffSetDays) / (double)7); i++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = rowHeight;
                MonthViewGrid.RowDefinitions.Add(rowDef);
            }
        }

        private void UpdateMonth(int MonthsToAdd)
        {
            MonthChangedEventArgs ev = new MonthChangedEventArgs();
            ev.OldDisplayStartDate = _DisplayStartDate;
            this.DisplayStartDate = _DisplayStartDate.AddMonths(MonthsToAdd);
            ev.NewDisplayStartDate = _DisplayStartDate;
            DisplayMonthChanged?.Invoke(ev);
            BuildCalendarUI();
        }

        private void UpdateYear(int YearsToAdd)
        {
            MonthChangedEventArgs ev = new MonthChangedEventArgs();
            ev.OldDisplayStartDate = _DisplayStartDate;
            this.DisplayStartDate = _DisplayStartDate.AddYears(YearsToAdd);
            ev.NewDisplayStartDate = _DisplayStartDate;
            DisplayMonthChanged?.Invoke(ev);
            BuildCalendarUI();
        }

        private void MonthGoPrev_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(-1);
        }

        private void MonthGoNext_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(1);
        }

        private void YearGoPrev_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateYear(-1);
        }

        private void YearGoNext_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateYear(1);
        }

        private void Appointment_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DayBoxAppointmentControl dbac = (DayBoxAppointmentControl)sender;
            if (e.Source.GetType() == typeof(DayBoxAppointmentControl))
            {
                if ((DayBoxAppointmentControl)e.Source != null)
                {
                    AppointmentDblClicked?.Invoke(dbac.m_doctor, dbac.m_startDate, dbac.m_endDate);
                }
                e.Handled = true;
            }
        }

        private void DayBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // -- call to FindVisualAncestor to make sure they didn't click on existing appointment (in which case,
            // that appointment window is already opened by handler Appointment_DoubleClick)
            if (e.Source.GetType() == typeof(DayBoxControl) && Utilities.FindVisualAncestor(typeof(DayBoxAppointmentControl), e.OriginalSource as Visual) == null)
            {
                NewAppointmentEventArgs ev = new NewAppointmentEventArgs();
                if ((DayBoxControl)e.Source != null) //Tag
                {
                    ev.StartDate = new DateTime(_DisplayYear, _DisplayMonth, System.Convert.ToInt32(((DayBoxControl)e.Source).m_day), 10, 0, 0); //Tag
                    ev.EndDate = (DateTime)ev.StartDate.Value.AddHours(2);
                }

                DayBoxDoubleClicked?.Invoke(ev);
                e.Handled = true;
            }
            
        }

        private void DayBox_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Source.GetType() == typeof(DayBoxControl) && Utilities.FindVisualAncestor(typeof(DayBoxAppointmentControl), e.OriginalSource as Visual) == null)
            {
                DayBoxControl dbc = (DayBoxControl)e.Source;
                dbc.Highlight();
            }
        }
    }
}
    public struct MonthChangedEventArgs
    {
        public DateTime OldDisplayStartDate;
        public DateTime NewDisplayStartDate;
    }

    public struct NewAppointmentEventArgs
    {
        public DateTime? StartDate;
        public DateTime? EndDate;
        public int? CandidateId;
        public int? RequirementId;
    }

    class Utilities
    {
        // -- Many thanks to Bea Stollnitz, on whose blog I found the original C# version of below in a drag-drop helper class... 
        public static FrameworkElement FindVisualAncestor(System.Type ancestorType, System.Windows.Media.Visual visual)
        {
            while ((visual != null && !ancestorType.IsInstanceOfType(visual)))
                visual = (System.Windows.Media.Visual)System.Windows.Media.VisualTreeHelper.GetParent(visual);
            return (FrameworkElement)visual;
        }
    }

