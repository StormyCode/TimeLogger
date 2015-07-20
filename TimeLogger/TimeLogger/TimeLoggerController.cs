﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger
{
    class TimeLoggerController
    {
        /// <summary>
        /// Pfad, an dem Daten gespeichert werden
        /// </summary>
        public string ExportDirectory { get; private set; }
        /// <summary>
        /// Name der Export-Datei
        /// </summary>
        public string ExportFileName { get; private set; }
        /// <summary>
        /// Liste aller LogItems
        /// </summary>
        public List<LogItem> LogList { get; private set; }

        public TimeLoggerController(string path)
        {
            this.ExportDirectory = path;
            this.ExportFileName = "log.txt";
            this.LogList = new List<LogItem>();
            ReadLogFile();
        }
        /// <summary>
        /// Überschreibt das LogFile mit allen Einträgen der LogList
        /// </summary>
        public void UpdateLogFile()
        {
            //Überprüft, ob Pfad und Datei gesetzt sind
            if (this.ExportDirectory != null && this.ExportFileName != null)
            {
                //Überprüft, ob Pfad exisitert und legt diesen gegebenfalls an
                if (!System.IO.Directory.Exists(this.ExportDirectory))
                    System.IO.Directory.CreateDirectory(this.ExportDirectory);
                //Legt einen StreamWriter fürs LogFile an (Modus = überschreiben)
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(String.Format("{0}/{1}", this.ExportDirectory, this.ExportFileName), false))
                {
                    //Schreibt jedes LogItem ins LogFile
                    foreach (LogItem item in this.LogList)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// Fügt der LogList ein neues LogItem hinzu, vorrausgesetzt die übergebenen Zeiten sind zulässig
        /// </summary>
        /// <param name="date">Datum des hinzuzufügenden LogItems</param>
        /// <param name="starttime">Startzeit des hinzuzufügenden LogItems</param>
        /// <param name="endtime">Endzeit des hinzuzufügenden LogItems</param>
        public void Log(DateTime date, string starttime, string endtime)
        {
            //Wenn Zeitangaben zulässig sind, füge zu LogList hinzu
            if (LogItem.Validate(date, starttime, endtime))
            {
                //Parse Startzeit
                TimeSpan start = new TimeSpan();
                TimeSpan.TryParse(starttime, out start);
                //Parse Endzeit
                TimeSpan end = new TimeSpan();
                TimeSpan.TryParse(endtime, out end);

                //Überprüft, ob bereits ein LogItems für angegebens Datum vorhanden ist
                bool exists = false;
                foreach (LogItem item in this.LogList)
                {
                    if (item.Date == date)
                    {
                        //ändert Werte falls vorhanden
                        exists = true;
                        item.Start = start;
                        item.End = end;
                        break;
                    }
                }
                //legt neues LogItem an, wenn noch nicht vorhanden
                if(!exists)
                    this.LogList.Add(new LogItem(date, start, end));

                //Updated das LogFile
                this.UpdateLogFile();
            }
        }
        /// <summary>
        /// Fügt der LogList ein LogItem hinzu, sofern noch kein Eintrag mit demselben Datum existiert. Ansonsten werden die Zeitwerte des bestehenden Eintrages überschrieben
        /// </summary>
        /// <param name="item">Das einzufügende LogItem</param>
        public void Log(LogItem item)
        {
            if (LogItem.Validate(item))
            {
                //Suche nach LogItem mit gleichem Datum
                for (int i = 0; i < this.LogList.Count; i++)
                {
                    if (this.LogList[i].Date == item.Date)
                    {
                        //Überschreibe des gefunden LogItems mit dem neuen LogItem
                        this.LogList[i] = item;
                        this.UpdateLogFile();
                        return;
                    }
                }
                //Falls kein LogItem mit gegebenem Datum vorhanden ist, wird das neue LogItem einfach der LogList hinzugefügt
                this.LogList.Add(item);
                this.UpdateLogFile();
            }
        }
        /// <summary>
        /// Liest LogFile ein, wenn existent
        /// </summary>
        public void ReadLogFile()
        {
            if (System.IO.File.Exists(String.Format("{0}/{1}", this.ExportDirectory, this.ExportFileName)))
                foreach (string line in System.IO.File.ReadAllLines(String.Format("{0}/{1}", this.ExportDirectory, this.ExportFileName)))
                {
                    this.Log(new LogItem(line));
                }
        }
        /// <summary>
        /// Errechnet die Gesamtzeitdifferenz für alle LogItems der LogList
        /// </summary>
        /// <returns>TimeSpan, die die Gesamtzeitdifferenz aller LogItems der Loglist darstellt</returns>
        public TimeSpan GetLogSum()
        {
            TimeSpan sum = new TimeSpan();
            foreach (LogItem item in this.LogList)
            {
                sum.Add(item.GetDifference());
            }
            return sum;
        }
        /// <summary>
        /// Gibt LogItem mit übereinstimmendem Datum zurück. Falls nicht vorhanden ist der Rückgabewert null
        /// </summary>
        /// <param name="date">Das zu übereinstimmende Datum</param>
        /// <returns></returns>
        public LogItem GetLogItemByDate(DateTime date)
        {
            foreach (LogItem item in this.LogList)
            {
                if (item.Date == date)
                    return item;
            }
            return null;
        }
    }
}
