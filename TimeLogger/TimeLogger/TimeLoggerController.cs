using System;
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
    }
}
