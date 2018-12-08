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
    public partial class EditAppointment: Form
    {
        public bool m_delete { get; set; }
        public bool m_changed { get; set; }
        public String m_doctor { get; set; }
        public DateTime m_startDate { get; set; }
        public EditAppointment()
        {
            InitializeComponent();
            m_delete = false;
            m_changed = false;
            m_doctor = "";
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            m_changed = true;
            m_doctor = doctorComboBox.Text;
            m_startDate = startTimePicker.Value;

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            m_changed = false;
            Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            String msg = "Are you sure you want to delete this appointment?";
            if(MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                m_delete = true;
                Close();
            }
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
