using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPSC481Project
{
    // TODO: this doesn't handle trying to edit appointment to an already filled timeslot
    public partial class AddVacation : Form
    {
        public bool m_add;
        public String m_doctor { get; set; }
        public DateTime m_startDate { get; set; }
        public DateTime m_endDate { get; set; }
        public AddVacation()
        {
            InitializeComponent();
            m_add = false;
            m_doctor = "";
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            m_add = true;
            m_doctor = doctorComboBox.Text;
            m_startDate = startTimePicker.Value;
            m_endDate = endTimePicker.Value;

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            m_add = false;
            Close();
        }

        public void SetInfo(String doc)
        {
            doctorComboBox.SelectedIndex = (doc == "Dr. Walter" ? 0 : doc == "Dr. Lee" ? 1 : 2);
            startTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.CustomFormat = "MM/dd/yyyy hh:mm";
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.CustomFormat = "MM/dd/yyyy hh:mm";
            startTimePicker.Value = DateTime.Today;
            endTimePicker.Value = DateTime.Today.AddDays(1);
        }

        private void startTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = startTimePicker.Value;
            TimeSpan d = TimeSpan.FromDays(1);
            startTimePicker.Value = new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        private void endTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = endTimePicker.Value;
            TimeSpan d = TimeSpan.FromDays(1);
            endTimePicker.Value = new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}
