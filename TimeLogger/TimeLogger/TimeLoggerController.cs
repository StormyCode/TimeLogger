using FirstFloor.ModernUI.Presentation;
using ModernUINavigation_Test.Pages.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TimeLogger.Models;

namespace TimeLogger
{
    public class TimeLoggerController
    {
        public TimeLoggerController() { }

        private static TimeLoggerController Tlc { get; set; }

        /// <summary>
        /// Gibt eine TLC Instanz zurück
        /// </summary>
        /// <returns>TimeLoggerController Instanz</returns>
        public static TimeLoggerController Instance
        {
            get
            {
                if (Tlc == null)
                    Tlc = Deserialize(@"Resources\timelogger.xml");
                return Tlc;
            }
        }

        public Settings Settings { get; set; }

        /// <summary>
        /// Liste aller LogItems
        /// </summary>
        public List<TimeLog> Log { get; set; }

        public List<VacationLog> VacationList { get; set; }
        public List<VacationType> VacationTypes { get; set; }

        public void Serialize()
        {
            XmlSerializer ser = new XmlSerializer(typeof(TimeLoggerController));
            XmlWriter writer = XmlWriter.Create(new FileStream(@"Resources\timelogger.xml", FileMode.Create), new XmlWriterSettings() { Indent = true, NewLineOnAttributes = false });
            ser.Serialize(writer, this);
        }

        public static TimeLoggerController Deserialize(string xmlFile)
        {
            StreamReader sr = new StreamReader(xmlFile);
            XmlSerializer ser = new XmlSerializer(typeof(TimeLoggerController));
            return (TimeLoggerController)ser.Deserialize(sr);
        }

        /// <summary>
        /// Fügt der LogList ein neues LogItem hinzu, vorrausgesetzt die übergebenen Zeiten sind zulässig
        /// </summary>
        /// <param name="date">Datum des hinzuzufügenden LogItems</param>
        /// <param name="starttime">Startzeit des hinzuzufügenden LogItems</param>
        /// <param name="endtime">Endzeit des hinzuzufügenden LogItems</param>
        public void WriteLog(DateTime date, string starttime, string endtime)
        {
            //Wenn Zeitangaben zulässig sind, füge zu LogList hinzu
            if (TimeLog.Validate(starttime) && TimeLog.Validate(endtime))
            {
                //Parse Startzeit
                TimeSpan start = new TimeSpan();
                TimeSpan.TryParse(starttime, out start);
                //Parse Endzeit
                TimeSpan end = new TimeSpan();
                TimeSpan.TryParse(endtime, out end);

                //Überprüft, ob bereits ein LogItems für angegebens Datum vorhanden ist
                bool exists = false;
                foreach (TimeLog item in Log)
                {
                    if (item.Date == date)
                    {
                        //ändert Werte falls vorhanden
                        exists = true;
                        item.Start = start.ToString(@"hh\:mm");
                        item.End = end.ToString(@"hh\:mm");
                        break;
                    }
                }
                //legt neues LogItem an, wenn noch nicht vorhanden
                if (!exists)
                    Log.Add(new TimeLog(date, start, end));

                //Updated das LogFile
                Serialize();
            }
        }

        /// <summary>
        /// Fügt der LogList ein LogItem hinzu, sofern noch kein Eintrag mit demselben Datum existiert. Ansonsten werden die Zeitwerte des bestehenden Eintrages überschrieben
        /// </summary>
        /// <param name="item">Das einzufügende LogItem</param>
        public void WriteLog(TimeLog item)
        {
            if (TimeLog.Validate(item))
            {
                //Suche nach LogItem mit gleichem Datum
                for (int i = 0; i < Log.Count; i++)
                {
                    if (Log[i].Date == item.Date)
                    {
                        //Überschreibe des gefunden LogItems mit dem neuen LogItem
                        Log[i] = item;
                        Serialize();
                        return;
                    }
                }
                //Falls kein LogItem mit gegebenem Datum vorhanden ist, wird das neue LogItem einfach der LogList hinzugefügt
                Log.Add(item);
                Serialize();
            }
        }

        public string GetLogSum()
        {
            TimeSpan sum = new TimeSpan();
            foreach (TimeLog item in Log.Where(x => x.End != "00:00"))
            {
                sum = sum.Add(item.GetDifference().Subtract(new TimeSpan(Instance.Settings.WorkingHoursPerDay, 0, 0)).Subtract(new TimeSpan(CountVacationType("Flextime") * Settings.WorkingHoursPerDay, 0, 0)));
            }
            return string.Format("{0}T {1}h {2}m", sum.Days, sum.Hours, sum.Minutes);
        }

        /// <summary>
        /// Gibt LogItem mit übereinstimmendem Datum zurück. Falls nicht vorhanden ist der Rückgabewert null
        /// </summary>
        /// <param name="date">Das zu übereinstimmende Datum</param>
        /// <returns></returns>
        public TimeLog GetLogItemByDate(DateTime date)
        {
            foreach (TimeLog item in Log)
            {
                if (item.Date == date)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Erstellt eine .csv Datei, in der alle LogItems geschrieben werden.
        /// Diese Datei wird am hinterlegten ExportDirectory gespeichert.
        /// </summary>
        public void SaveLogFileToFile()
        {
            using (StreamWriter sw = new StreamWriter(Settings.ExportDirectory + DateTime.Today.ToShortDateString() + @"_Export_Log.csv", false))
            {
                //sw.WriteLine("Datum;Startzeit;Endzeit;Zeitdifferenz");
                foreach (TimeLog item in Log)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }


        /// <summary>
        /// Methode, die die Anzahl der verbleibenden Urlaubstage zurückgibt.
        /// </summary>
        /// <returns>Anzahl der verbleibenden Urlaubstage</returns>
        public int GetRemainingVacationDays()
        {
            return Settings.DaysVacationPerYear - CountVacationType("Vacation");
        }

        public int CountVacationType(string type)
        {
            return VacationList.Count(x => x.Type.Name.ToLower() == type.ToLower() && x.Date.Year == DateTime.Today.Year);
        }

        public void UpdateVacationList(DateTime dt, VacationType type)
        {
            bool found = false;
            foreach (VacationLog vlog in VacationList)
            {
                if (vlog.Date.Date == dt.Date)
                {
                    found = true;
                    vlog.Type = type;
                    break;
                }
            }
            if (!found)
                VacationList.Add(new VacationLog() { Date = dt, Type = type });

            VacationList = VacationList.Where(x => x.Type != null && x.Type.Name != "Work").ToList();
            Serialize();
        }

        public VacationType GetDateVacationType(DateTime dt)
        {
            foreach (VacationLog log in VacationList)
            {
                if (log.Date.Date == dt.Date)
                {
                    return log.Type;
                }
            }
            return null;
        }

        public VacationType GetVacationTypeByName(string name)
        {
            foreach (VacationType type in VacationTypes)
            {
                if (type.Name == name)
                    return type;
            }
            return null;
        }
    }
}
