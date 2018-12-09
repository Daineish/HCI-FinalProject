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
    public partial class AddAppointment : Form
    {
        public bool m_add { get; set; }
        public String m_doctor { get; set; }
        public DateTime m_startDate { get; set; }
        public AddAppointment()
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

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            m_add = false;
            Close();
        }

        public void SetInfo(String name, String num, String doc, DateTime start)
        {
            patientHCNum.Text = num;
            patientName.Text = name;
            doctorComboBox.SelectedIndex = (doc == "Dr. Walter" ? 0 : doc == "Dr. Lee" ? 1 : 2);
            startTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.CustomFormat = "MM/dd/yyyy hh:mm";
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.CustomFormat = "MM/dd/yyyy hh:mm";
            startTimePicker.Value = start;
            endTimePicker.Value = start.AddMinutes(10);
        }

        private void startTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = startTimePicker.Value;
            TimeSpan d = TimeSpan.FromMinutes(10);
            startTimePicker.Value = new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
            endTimePicker.Value = startTimePicker.Value.AddMinutes(10);
        }
    }
}
