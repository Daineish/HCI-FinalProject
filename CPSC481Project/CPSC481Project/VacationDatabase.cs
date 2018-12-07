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
    class VacationDatabase
    {
        private static String m_vacationFile = "./Vacations.txt";
        JsonSerializer m_serializer;
        public List<Vacation> m_vacationList;

        public VacationDatabase()
        {
            if (!File.Exists(m_vacationFile))
            {
                // idk if this is necessary because idk how files work in C#.
                File.Create(m_vacationFile).Close();
            }

            StreamReader vacationFile = File.OpenText(m_vacationFile);

            m_serializer = new JsonSerializer();
            m_vacationList = (List<Vacation>)(m_serializer.Deserialize(vacationFile, typeof(List<Vacation>)));
            if (m_vacationList == null)
            {
                m_vacationList = new List<Vacation>();
            }
        }

        public int NumVacations()
        {
            return m_vacationList.Count();
        }

        public void AddVacation(Vacation v)
        {
            m_vacationList.Add(v);
        }

        public void AddVacation(String doc, DateTime start, DateTime end)
        {
            Vacation v = new Vacation(doc, start, end);
            m_vacationList.Add(v);
        }

        // not implemented yet
        public Boolean RemoveVacation(Vacation v)
        {
            Console.WriteLine("Remove vacations is not implemented yet!");
            Boolean rv = false;
            //foreach (Vacation v1 in m_vacationList)
            //{
            //    if (v.m_doctor == v1.m_doctor)
            //    {
            //        m_vacationList.Remove(p);
            //        rv = true;
            //        break;
            //    }
            //}
            return rv;
        }

        public void PrintAllVacations()
        {
            Console.WriteLine("All Vacations: " + m_vacationList.Count() + ".");
            Console.WriteLine("------------------------------------------");
            foreach (Vacation v in m_vacationList)
            {
                v.PrintVacation();
            }
            Console.WriteLine("------------------------------------------");
        }

        ~VacationDatabase()
        {
            String json = JsonConvert.SerializeObject(m_vacationList, Formatting.Indented);
            File.WriteAllText(m_vacationFile, json);
        }


    }
}
