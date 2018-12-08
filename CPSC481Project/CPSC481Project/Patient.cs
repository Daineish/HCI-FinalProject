using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPSC481Project
{
    /**
     *  Represents a patient.
     */
    public class Patient
    {
        public String m_lastName { get; set; }
        public String m_firstName { get; set; }
        public String m_hcNumber { get; set; } // uint? or string?
        public String m_address { get; set; }
        public String m_email { get; set; }
        public String m_phoneNumber { get; set; }

        public Patient()
        {
            // throw error
        }

        public Patient(String last, String first, String num, String addy, String email, String phone)
        {
            m_lastName = last;
            m_firstName = first;
            m_hcNumber = num;
            m_address = addy;
            m_email = email;
            m_phoneNumber = phone;
        }

        public Boolean MatchHealthCareNumber(String num)
        {
            // Doing String->num then compare is probably better.
            return num == m_hcNumber;
        }

        public Boolean MatchName(String name)
        {
            return false; // probably don't need this.
            //return name == m_name;
        }

        public String GetHCNumber() { return m_hcNumber; }
        public String GetLastName() { return m_lastName; }
        public String GetFirstName() { return m_firstName; }
        public String GetAddress() { return m_address; }
        public String GetEmail() { return m_email; }
        public String GetPhone() { return m_phoneNumber; }

        // Debug method to print patient info
        public void PrintPatientInfo()
        {
            Console.WriteLine("Patient: \"" + m_hcNumber + "\", \"" + m_lastName + ", " + m_firstName + "\", \"" +
                               m_address + "\", \"" + m_email + "\", \"" + m_phoneNumber + "\"");
        }
    }
}
