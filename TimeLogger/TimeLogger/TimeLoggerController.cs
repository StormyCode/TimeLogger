using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;

namespace TimeLogger
{
    class TimeLoggerController
    {
        private static TimeLoggerController Tlc { get; set; }

        /// <summary>
        /// Pfad, an dem Daten gespeichert werden
        /// </summary>
        public string ExportDirectory { get; private set; }

        /// <summary>
        /// Name der Export-Datei
        /// </summary>
        public string ExportFileName { get; private set; }

        #region All about LogList
        /// <summary>
        /// Liste aller LogItems
        /// </summary>
        public List<LogItem> LogList { get; private set; }
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
                if (!exists)
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
        public string GetLogSum()
        {
            TimeSpan sum = new TimeSpan();
            foreach (LogItem item in this.LogList)
            {
                sum = sum.Add(item.GetDifference());
            }
            return String.Format("{0}T {1}h {2}m", sum.Days, sum.Hours, sum.Minutes);
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
        /// <summary>
        /// Erstellt eine .csv Datei, in der alle LogItems geschrieben werden.
        /// Diese Datei wird am hinterlegten ExportDirectory gespeichert.
        /// </summary>
        public void SaveLogFileToFile()
        {
            using (StreamWriter sw = new StreamWriter(this.ExportDirectory + DateTime.Today.ToShortDateString() + @"_Export_Log.csv", false))
            {
                sw.WriteLine("Datum;Startzeit;Endzeit;Zeitdifferenz");
                foreach (LogItem item in this.LogList)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }
        #endregion

        #region All about Settings
        /// <summary>
        /// Beinhaltet ein Dictionary mit allen Einstellungen zum einer Tlc Instanz
        /// </summary>
        public Dictionary<string, string> Settings { get; private set; }
        /// <summary>
        /// Liest die Einstellungen aus der app.config aus
        /// </summary>
        private void ReadSettings()
        {
            if (File.Exists(this.ExportDirectory + "/settings.csv"))
            {
                //bestehende Eintellungen einlesen
                foreach (string line in File.ReadAllLines(this.ExportDirectory + "/settings.csv"))
                {
                    string[] properties = line.Replace(" ", "").Split(new char[] { ';' });
                    if (this.Settings.ContainsKey(properties[0]))
                        this.Settings[properties[0]] = properties[1];
                    else
                        this.Settings.Add(properties[0], properties[1]);
                }
            }
            //Default Einstellunge setzen, falls kein Wert vordefiniert
            if (this.Settings.ContainsKey("duration_lunchtime") == false)
                this.UpdateSetting("duration_lunchtime", "1");
            if (this.Settings.ContainsKey("working_hours") == false)
                this.UpdateSetting("working_hours", "7");
            if (this.Settings.ContainsKey("vacation_per_year") == false)
                this.UpdateSetting("vacation_per_year", "7");
            if (this.Settings.ContainsKey("doubleclick_autoinsert_timespan") == false)
                this.UpdateSetting("doubleclick_autoinsert_timespan", "7");

            this.ApplySettings();
        }
        /// <summary>
        /// Methode, die alle aktuellen Einstellungen in eine Datei schreibt
        /// </summary>
        public void WriteSettings()
        {
            if (!File.Exists(this.ExportDirectory + "/settings.csv"))
                File.Create(this.ExportDirectory + "/settings.csv");
            List<string> text = new List<string>();
            foreach (KeyValuePair<string, string> pair in this.Settings)
            {
                text.Add(String.Format("{0};{1}", pair.Key, pair.Value));
            }
            File.WriteAllLines(this.ExportDirectory + "/settings.csv", text.ToArray());
        }
        /// <summary>
        /// Methode, die alle Einstellungen wirksam macht
        /// </summary>
        private void ApplySettings()
        {
            if (this.Settings.ContainsKey("accent_color"))
                AppearanceManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString(this.Settings["accent_color"]);
            if (this.Settings.ContainsKey("theme"))
                AppearanceManager.Current.ThemeSource = new System.Uri(this.Settings["theme"]);
        }
        /// <summary>
        /// Methode, die das Updaten von Einstellungen regelt
        /// </summary>
        /// <param name="key">Name der Einstellung</param>
        /// <param name="val">Wert der Einstellung</param>
        public void UpdateSetting(string key, string val)
        {
            if (this.Settings.ContainsKey(key))
                this.Settings[key] = val;
            else
                this.Settings.Add(key, val);
            this.ApplySettings();
            this.WriteSettings();
        }
        #endregion

        #region All about Vacation
        public Dictionary<DateTime, VacationType> VacationList { get; private set; }
        public enum VacationType { Vacation, Flexitime }

        /// <summary>
        /// Methode, die die Anzahl der verbleibenden Urlaubstage zurückgibt.
        /// </summary>
        /// <returns>Anzahl der verbleibenden Urlaubstage</returns>
        public int GetRemainingVacationDays()
        {
            return int.Parse(this.Settings["vacation_per_year"]) - this.VacationList.Where(x => x.Value == VacationType.Vacation).Count();
        }

        public void UpdateVacationList(DateTime dt, string type)
        {
            //TODO: Julian - implement logic here
        }
        #endregion

        /// <summary>
        /// Gibt eine TLC Instanz zurück
        /// </summary>
        /// <returns>TimeLoggerController Instanz</returns>
        public static TimeLoggerController GetInstance()
        {
            if (Tlc == null)
                Tlc = new TimeLoggerController();
            return Tlc;
        }

        /// <summary>
        /// Initialisiert einen TLC
        /// </summary>
        private TimeLoggerController()
        {
            this.LogList = new List<LogItem>();
            this.Settings = new Dictionary<string, string>();
            this.VacationList = new Dictionary<DateTime, VacationType>();
            //AppearanceManager.Current.AccentColor = Colors.Green;
            this.ExportDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            this.ReadSettings();
            //HACK: Hard coded paths
            this.ExportFileName = "log.txt";

            this.ReadLogFile();
        }



    }
}
