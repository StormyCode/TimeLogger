using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger
{
    class LogItem
    {
        /// <summary>
        /// Stellt das Datum des LogItems dar
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Stellt die Startuhrzeit des LogItems dar
        /// </summary>
        public TimeSpan Start { get; set; }
        /// <summary>
        /// Stellt die Enduhrzeit des LogItems dar
        /// </summary>
        public TimeSpan End { get; set; }
        /// <summary>
        /// Legt eine neue Instanz von LogItem mit bestimmtem Datum und Zeiten an
        /// </summary>
        /// <param name="date">Datum des LogItems</param>
        /// <param name="starttime">Startzeit des LogItems</param>
        /// <param name="endtime">Endzeit des LogItems</param>
        public LogItem(DateTime date, TimeSpan starttime, TimeSpan endtime)
        {
            this.Date = date;
            this.Start = starttime;
            this.End = endtime;
        }
        /// <summary>
        /// Legt eine neue Instanz von LogItem anhand eines Strings des Formats "Datum;Startzeit;Endzeit" an
        /// </summary>
        /// <param name="str"></param>
        public LogItem(string str)
        {
            string[] data = str.Split(';');
            DateTime date = new DateTime();
            DateTime.TryParse(data[0], out date);
            this.Date = date;
            TimeSpan start = new TimeSpan();
            TimeSpan.TryParse(data[1], out start);
            this.Start = start;
            TimeSpan end = new TimeSpan();
            TimeSpan.TryParse(data[2], out end);
            this.End = end;
        }
        /// <summary>
        /// Gibt das LogItem in "Datum;Startzeit;Endzeit" Format zurück
        /// </summary>
        /// <returns>String, der das LogItems darstellt</returns>
        public override string ToString()
        {
            return String.Format("{0};{1};{2}", this.Date, this.Start, this.End);
        }
        /// <summary>
        /// Gibt die Differenz zwischen Start- und Endzeit zurück
        /// </summary>
        /// <returns>TimeSpan, der die Differenz zwischen Start- und Endzeit widerspiegelt</returns>
        public TimeSpan GetDifference()
        {
            return this.End.Subtract(this.Start);
        }
        /// <summary>
        /// Statische Methode, die ermittelt, ob das Datum und die Zeitangaben zulässig sind
        /// </summary>
        /// <param name="date">Datum des LogItems</param>
        /// <param name="starttime">Startzeit des LogItems</param>
        /// <param name="endtime">Endzeit des LogItems</param>
        /// <returns></returns>
        public static bool Validate(DateTime date, string starttime, string endtime)
        {
            bool valid = true;
            TimeSpan start = new TimeSpan();
            valid &= TimeSpan.TryParse(starttime, out start);
            TimeSpan end = new TimeSpan();
            valid &= TimeSpan.TryParse(endtime, out end);
            return valid;
        }
    }
}
