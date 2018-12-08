using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPSC481Project
{
    public class Appointment
    {
        //from other class
        private int _AppointmentID;
        private string _Subject;
        private System.Nullable<DateTime> _StartTime;
        private System.Nullable<DateTime> _EndTime;
        private DateTime _reccreatedDate;

        //public uint m_apptNumber {  get; set; }
        public Patient m_patient { get; set; }
        public String m_doctor { get; set; }
        public DateTime m_startTime { get; set; }
        public DateTime m_endTime { get; set; }
        public String m_information { get; set; }

        //private static uint numOfAppointments = 0;

        public Appointment()
        {
            // throw error or something
        }

        public Appointment(Patient pat, String doc, DateTime start, DateTime end, String info)
        {
            m_patient = pat;
            m_doctor = doc;
            m_startTime = start;
            m_endTime = end;
            m_information = info;
            //m_apptNumber = numOfAppointments++; // or something? Need some way of identifying appointments.
        }

        /* FROM OTHER CLASS */
        public int AppointmentID
        {
            get
            {
                return this._AppointmentID;
            }
            set
            {
                if (((this._AppointmentID == value) == false))
                    this._AppointmentID = value;
            }
        }

        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                if ((string.Equals(this._Subject, value) == false))
                    this._Subject = value;
            }
        }

        public System.Nullable<DateTime> StartTime
        {
            get
            {
                return this._StartTime;
            }
            set
            {
                if ((this._StartTime.Equals(value) == false))
                    this._StartTime = value;
            }
        }

        public System.Nullable<DateTime> EndTime
        {
            get
            {
                return this._EndTime;
            }
            set
            {
                if ((this._EndTime.Equals(value) == false))
                    this._EndTime = value;
            }
        }

        public DateTime reccreatedDate
        {
            get
            {
                return this._reccreatedDate;
            }
            set
            {
                if (((this._reccreatedDate == value) == false))
                    this._reccreatedDate = value;
            }
        }
        /*END FROM OTHER CLASS*/


        /**
         * Debugging function used to print appointments to console.
         */
        public void PrintAppointment()
        {
            //Console.WriteLine("Number: " + m_apptNumber);
            Console.WriteLine("Appointment\n-----------");
            Console.WriteLine("\tPatientID: " + m_patient.GetHCNumber());
            Console.WriteLine("\tDoctor: " + m_doctor);
            Console.WriteLine("\tStartTime: " + m_startTime);
            Console.WriteLine("\tEndTime: " + m_endTime);
            Console.WriteLine("\tInfo: " + m_information);
            Console.WriteLine("\n");
        }
    }
}
