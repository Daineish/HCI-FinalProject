using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace CPSC481Project
{
    /**
     * An /easy/ way to keep track of all patients.
     */
    class PatientDatabase
    {
        // Patient list where key is HC#.
        List<Patient> m_patientList;
        private static String m_patientFile = "./Patients.txt";
        JsonSerializer m_serializer;

        public PatientDatabase()
        {
            if (!File.Exists(m_patientFile))
            {
                // idk if this is necessary because idk how files work in C#.
                File.Create(m_patientFile).Close();

            }

            StreamReader appointmentsFile = File.OpenText(m_patientFile);

            m_serializer = new JsonSerializer();
            m_patientList = (List<Patient>)(m_serializer.Deserialize(appointmentsFile, typeof(List<Patient>)));
            if (m_patientList == null)
            {
                m_patientList = new List<Patient>();
            }

        }

        ~PatientDatabase()
        {
            String json = JsonConvert.SerializeObject(m_patientList, Formatting.Indented);
            File.WriteAllText(m_patientFile, json);

        }

        public void AddPatient(String last, String first, String num, String addy, String email, String phone)
        {
            Patient pat = new Patient(last, first, num, addy, email, phone);
            m_patientList.Add(pat);
        }

        public void AddPatient(Patient p)
        {
            // TODO: Exception Handling.
            try
            {
                m_patientList.Add(p);
            }
            catch(Exception e)
            {
                // Popup warning or something
                Console.WriteLine("FAILED TO ADD PATIENT");
                Console.WriteLine(e.Message);
            }
        }

        public Boolean RemovePatient(String num)
        {
            Boolean rv = false;
            foreach(Patient p in m_patientList)
            {
                if(p.m_hcNumber == num)
                {
                    m_patientList.Remove(p);
                    rv = true;
                }
            }
            return rv;
        }

        public void RemoveAllPatients()
        {
            m_patientList.Clear();
        }
        /*
         * Return a patient with corresponding HC
         * 
         * 
         */ 
        public Patient findPatient(String h)
        {
            //Patient temp = new Patient();
            //try
            //{
            //    temp = m_patientList[h];
            //}
            //catch (Exception e)
            //{
            //    //temp = null;
            //    // FileNotFoundExceptions are handled here.
            //}
            //return temp;
            List<Patient> l = FindPatientHC(h);
            if (l.Count() == 0)
                return null;
            else if (l.Count == 1)
                return l[0];
            else
                return l[0];
        }
         
        /**
         * Searches the patient list and returns a list of patients whose HC# contains num.
         */
        public List<Patient> FindPatientHC(String num)
        {
            if (num.Length == 0) return m_patientList;
            List<Patient> rv = new List<Patient>();
            foreach(Patient p in m_patientList)
            {
                if (p.m_hcNumber.Contains(num))
                    rv.Add(p);
            }
            return rv;
        }

        /**
         * Searches the patient list and returns a list of patients whose name contains name.
         */
        public List<Patient> FindPatientName(String name)
        {
            if (name.Length == 0) return m_patientList;
            List<Patient> rv = new List<Patient>();
            name = name.ToLower();
            foreach (Patient p in m_patientList)
            {
                if (p.m_lastName.ToLower().Contains(name) || p.m_firstName.ToLower().Contains(name))
                    rv.Add(p);
            }
            return rv;
        }
        public Boolean IsEmpty()
        {
            return m_patientList.Count() == 0;
        }

        public int NumPatients()
        {
            return m_patientList.Count();
        }

        // Debugging function to print all patients.
        public void PrintAllPatients()
        {
            Console.WriteLine("All Patients");
            Console.WriteLine("------------------------------------------");
            foreach (Patient p in m_patientList)
            {
                p.PrintPatientInfo();
            }
            Console.WriteLine("------------------------------------------");
        }
    }
}
