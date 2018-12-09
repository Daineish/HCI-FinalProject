using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPSC481Project
{
    public class Vacation
    {
        public String m_doctor { get; set; }
        public DateTime m_startDate { get; set; }
        public DateTime m_endDate { get; set; }
        public DateTime m_curDate { get; set; }

        public Vacation()
        {
            // error
        }

        public Vacation(String doc, DateTime start, DateTime end, DateTime cur)
        {
            m_doctor = doc;
            m_startDate = start;
            m_endDate = end;
            m_curDate = cur;
        }

        public Vacation(String doc, DateTime start, DateTime end)
        {
            m_doctor = doc;
            m_startDate = start;
            m_endDate = end;
            m_curDate = DateTime.Today;
        }

        public void PrintVacation()
        {
            //Console.WriteLine("Number: " + m_apptNumber);
            Console.WriteLine("Vacation\n-----------");
            Console.WriteLine("\tDoctor: " + m_doctor);
            Console.WriteLine("\tStartTime: " + m_startDate);
            Console.WriteLine("\tEndTime: " + m_endDate);
            Console.WriteLine("\n");
        }
    }
}
