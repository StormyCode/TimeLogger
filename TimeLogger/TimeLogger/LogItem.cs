﻿using System;
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
    }
}
