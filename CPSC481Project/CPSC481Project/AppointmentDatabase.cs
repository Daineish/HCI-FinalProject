﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using Microsoft.Win32;

namespace CPSC481Project
{
    public class AppointmentDatabase
    {

        /**
          * A dictionary of appointments. The key of each pair is the ID of the appointment, the
          * value of each pair is the Appointment object.
        */
        //private Dictionary<uint, Appointment> m_appointments;
        public List<Appointment> m_appointments { get; }
        private JsonSerializer m_serializer;
        private static String m_appointmentFile = "./Appointments.txt";

        public AppointmentDatabase()
        {
            if(!File.Exists(m_appointmentFile))
            {
                // idk if this is necessary because idk how files work in C#.

                File.Create(m_appointmentFile).Close();
                
            }

            File.WriteAllText(m_appointmentFile, String.Empty);
            StreamReader appointmentsFile = File.OpenText(m_appointmentFile);
            //m_appointments = new Dictionary<uint, Appointment>();

            m_serializer = new JsonSerializer();
            m_appointments = (List<Appointment>)(m_serializer.Deserialize(appointmentsFile, typeof(List<Appointment>)));
            if (m_appointments == null)
                m_appointments = new List<Appointment>();

            UpdateAppointments();
        }

        public void AddAppointment(Appointment a)
        {
            m_appointments.Add(a);
            UpdateAppointments();
        }

        public void DeleteAppointment(Appointment a)
        {
            if (m_appointments.Remove(a))
            {
                // Successful message
            }
            else
            {
                // Item was not found in list, error message
            }
            UpdateAppointments();
        }

        public Boolean IsEmpty()
        {
            UpdateAppointments();
            return m_appointments.Count() == 0;
        }

        public int NumAppointments()
        {
            UpdateAppointments();
            return m_appointments.Count();
        }

        // Debug function to print all appointments.
        public void PrintAllAppointments()
        {
            Console.WriteLine("All Appointments: " + m_appointments.Count() + ".");
            Console.WriteLine("------------------------------------------");
            foreach (Appointment a in m_appointments)
            {
                a.PrintAppointment();
            }
            Console.WriteLine("------------------------------------------");
        }

        /**
         * Returns the #num soonest appointments by m_startTime.
         * 
         * If num > NumAppointments() returns m_appointments
         */
        //public List<Appointment> NextAppointments(int num)
        //{
        //    UpdateAppointments();
        //    // TODO: This hasn't been tested at all so odds are it doesn't work.
        //    if (num > NumAppointments())
        //    {
        //        return m_appointments;
        //    }

        //    List<Appointment> rv = new List<Appointment>();
        //    List<Appointment> remaining = new List<Appointment>();
        //    foreach(Appointment a in m_appointments)
        //    {
        //        remaining.Add(new Appointment(a.m_patient, a.m_doctor, a.StartTime.Value, a.EndTime.Value, a.m_information));
        //    }
        //    for(int i = 0; i < num; i++)
        //    {
        //        Appointment soonest = remaining.ElementAt(0);
        //        foreach(Appointment a in remaining)
        //        {
        //            if (DateTime.Compare(a.m_startTime, soonest.m_startTime) < 0)
        //                soonest = a;
        //        }
        //        rv.Add(soonest);
        //        remaining.Remove(soonest);
        //    }

        //    return rv;
        //}

        /**
         * Returns the appointment in m_appointments that has the soonest startTime.
         * 
         * If m_appointments is empty, returns null.
         */
        //public Appointment NextAppointment()
        //{
        //    UpdateAppointments();
        //    if (m_appointments.Count() == 0)
        //        return null;

        //    Appointment rv = m_appointments.ElementAt(0);
        //    foreach(Appointment a in m_appointments)
        //    {
        //        if (DateTime.Compare(a.m_startTime, rv.m_startTime) < 0)
        //            rv = a;
        //    }
        //    return rv;
        //}

        /**
         * Returns the appointment in m_appointments that has the soonest startTime where
         * the appointment's doctor == doc.
         * 
         * If no appointment in m_appointments has doc, returns null.
         */
        //public Appointment NextAppointment(String doc)
        //{
        //    UpdateAppointments();
        //    // TODO: Hasn't been tested.
        //    // TODO: Perhaps we should make a doctor class?
        //    Appointment rv = null;
        //    foreach (Appointment a in m_appointments)
        //    {
        //        if (a.m_doctor == doc)
        //        {
        //            rv = a;
        //            break;
        //        }
        //    }

        //    if(rv != null)
        //    {
        //        foreach (Appointment a in m_appointments)
        //        {
        //            if (a.m_doctor == doc)
        //            {
        //                if (DateTime.Compare(a.m_startTime, rv.m_startTime) < 0)
        //                    rv = a;
        //            }
        //        }
        //    }

        //    return rv;
        //}

        /**
         * Returns num appointments in m_appointments that have the soonest startTime where the
         * appointment's doctor == doc.
         * 
         * If there are less than num appointments that fit this description, it should return
         * a list of appointments with a size < num, or return an empty list. Maybe.
         */
        public List<Appointment> NextAppointments(String doc, int num)
        {
            UpdateAppointments();
            List<Appointment> rv = new List<Appointment>();
            List<Appointment> remaining = new List<Appointment>();
            foreach (Appointment a in m_appointments)
            {
                remaining.Add(new Appointment(a.m_patient, a.m_doctor, a.m_startTime, a.m_endTime, a.m_information));
            }

            for (int i = 0; i < num; i++)
            {
                Appointment soonest = null;
                DateTime soonestTime = DateTime.MaxValue;
                foreach (Appointment a in remaining)
                {
                    if (DateTime.Compare(a.m_startTime, DateTime.Today.AddDays(1)) >= 0)
                        continue;
                    if (DateTime.Compare(a.m_startTime, soonestTime) < 0 && a.m_doctor == doc)
                    {
                        soonest = a;
                        soonestTime = soonest.m_startTime;
                    } 
                }
                if(soonest != null)
                {
                    rv.Add(soonest);
                    remaining.Remove(soonest);
                }
            }

            return rv;
        }

        /**
         * Gets the next times where doc does not have any appointments scheduled.
         * Each element of the return list is a string saying what time's they're available?
         * 
         * @param doc: the doctor to find available times for.
         * @param num: the number of available times to retrieve. ((NOT IMPLEMENTED))
         * 
         * 
         * // TODO: this is friggin difficult with our current setup.
         * // How are we going to handle doctor vacations and stuff?
         * // This is *finished* but almost certainly doesn't work.
         */
        public List<String> AvailableTimes(String doc, int num = 2)
        {
            UpdateAppointments();
            List<Appointment> apts = new List<Appointment>();
            // Get all appointments for this doctor.
            foreach(Appointment a in m_appointments)
            {
                if (a.m_doctor == doc)
                    apts.Add(a);
            }

            List<String> rv = new List<String>();
            // If doc has no scheduled appointments, they're always available.

            if(apts.Count() == 0 )
            {
                rv.Add("Unavailable");
                rv.Add("Unavailable");

            }
            else
            {
                // I'm hardcoding this so it always returns 2 slots just because I'm lazy.

                // Sketch af algorithm.
                DateTime curTime = DateTime.Now;
                TimeSpan ts = TimeSpan.FromMinutes(10);
                curTime = new DateTime((curTime.Ticks + ts.Ticks - 1) / ts.Ticks * ts.Ticks, curTime.Kind);
                int i = 0;
                while(true)
                {
                    Boolean hasAppt = false;
                    Boolean ffTmr = false;
                    Boolean ffTdy = false;
                    foreach (Appointment a in apts)
                    {
                        if(DateTime.Compare(curTime, a.m_startTime) >= 0 && DateTime.Compare(curTime, a.m_endTime) <= 0)
                        {
                            // Has appointment at curTime -> break from loop and try 10 mins later.
                            hasAppt = true;
                            break;
                        }
                        if(curTime.Hour > 18)
                        {
                            ffTmr = true;
                            break;
                        }
                        if(curTime.Hour < 8)
                        {
                            ffTdy = true;
                            break;
                        }
                    }
                    if(hasAppt)
                        curTime = curTime.AddMinutes(10);
                    else if(ffTmr)
                    {
                        curTime = curTime.AddDays(1);
                        curTime = curTime.Date + new TimeSpan(8, 0, 0);
                    }
                    else if(ffTdy)
                    {
                        curTime = curTime.Date + new TimeSpan(8, 0, 0);
                    }
                    else
                    {
                        if (i == 0) // Found the first available time
                        {
                            String s = curTime.ToString("MMM. dd");
                            s += " " + curTime.ToString("t");
                            rv.Add(s);
                            i++;
                        }
                        else if (i == 1) // Found the second available time, but already taken by first available time
                        {
                            curTime = curTime.AddMinutes(10);
                            i++;
                        }
                        else // found the second available time
                        {
                            String s = curTime.ToString("MMM. dd");
                            s += " " + curTime.ToString("t");
                            rv.Add(s);
                            break;
                        }
                    }
                }
            }

            return rv;
        }

        public void UpdateAppointments()
        {
            List<Appointment> temp = new List<Appointment>();
            foreach(Appointment a in m_appointments)
            {
                if(DateTime.Compare(a.m_startTime, DateTime.Now) < 0)
                {
                    temp.Add(a);
                }
            }
            foreach(Appointment a in temp)
            {
                a.m_patient.m_prevDr = a.m_doctor;
                DeleteAppointment(a);
            }
        }

        ~AppointmentDatabase()
        {
            UpdateAppointments();
            String json = JsonConvert.SerializeObject(m_appointments, Formatting.Indented);
            File.WriteAllText(m_appointmentFile, json);
        }

    }
}
