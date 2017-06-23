using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.Models
{
    public class TimeLog
    {
        /// <summary>
        /// Stellt das Datum des LogItems dar
        /// </summary>
        [XmlAttribute]
        public DateTime Date { get; set; }
        /// <summary>
        /// Stellt die Startuhrzeit des LogItems dar
        /// </summary>
        [XmlAttribute]
        public string Start { get; set; }
        /// <summary>
        /// Stellt die Enduhrzeit des LogItems dar
        /// </summary>
        [XmlAttribute]
        public string End { get; set; }

        public TimeLog() { }

        /// <summary>
        /// Legt eine neue Instanz von LogItem mit bestimmtem Datum und Zeiten an
        /// </summary>
        /// <param name="date">Datum des LogItems</param>
        /// <param name="starttime">Startzeit des LogItems</param>
        /// <param name="endtime">Endzeit des LogItems</param>
        public TimeLog(DateTime date, TimeSpan starttime, TimeSpan endtime)
        {
            Date = date;
            Start = starttime.ToString(@"hh\:mm");
            End = endtime.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Legt eine neue Instanz von LogItem anhand eines Strings des Formats "Datum;Startzeit;Endzeit" an
        /// </summary>
        /// <param name="str"></param>
        public TimeLog(string str)
        {
            string[] data = str.Split(';');
            DateTime date = new DateTime();
            DateTime.TryParse(data[0], out date);
            Date = date;
            TimeSpan start = new TimeSpan();
            TimeSpan.TryParse(data[1], out start);
            Start = start.ToString(@"hh\:mm");
            TimeSpan end = new TimeSpan();
            TimeSpan.TryParse(data[2], out end);
            End = end.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Gibt das LogItem in "Datum;Startzeit;Endzeit" Format zurück
        /// </summary>
        /// <returns>String, der das LogItems darstellt</returns>
        public override string ToString()
        {
            return string.Format("{0};{1};{2}", Date, Start, End);
        }

        /// <summary>
        /// Gibt die Differenz zwischen Start- und Endzeit zurück
        /// </summary>
        /// <returns>TimeSpan, der die Differenz zwischen Start- und Endzeit widerspiegelt</returns>
        public TimeSpan GetDifference()
        {
            int lunchtimeHours = TimeLoggerController.Instance.Settings.DurationLunchTime;
            TimeSpan start = new TimeSpan();
            TimeSpan.TryParse(Start, out start);
            TimeSpan end = new TimeSpan();
            TimeSpan.TryParse(End, out end);
            return (end.Subtract(start)).Subtract(new TimeSpan(lunchtimeHours, 0, 0));
        }

        /// <summary>
        /// Statische Methode, die ermittelt, ob das Datum und die Zeitangaben zulässig sind
        /// </summary>
        /// <param name="date">Datum des LogItems</param>
        /// <param name="starttime">Startzeit des LogItems</param>
        /// <param name="time">Endzeit des LogItems</param>
        /// <returns></returns>
        public static bool Validate(string time)
        {
            if (string.IsNullOrEmpty(time)) return true;
            TimeSpan end = new TimeSpan();
            return TimeSpan.TryParse(time, out end);
        }

        /// <summary>
        /// Statische Methode, die ermittelt, ob das Datum und die Zeitangaben zulässig sind
        /// </summary>
        /// <param name="item">Das überprüfende LogItem</param>
        /// <returns></returns>
        public static bool Validate(TimeLog item)
        {
            return Validate(item.Start) && string.IsNullOrEmpty(item.End) ? true : Validate(item.End);
        }
    }
}
