using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TimeLogger.Models
{
    public class Settings
    {
        [XmlAttribute]
        public string AccentColor { get; set; }
        [XmlAttribute]
        public string ThemeColor { get; set; }
        [XmlAttribute]
        public int AutoInsertTime { get; set; }
        [XmlAttribute]
        public string ExportDirectory { get; set; }
        [XmlAttribute]
        public int DurationLunchTime { get; set; }
        [XmlAttribute]
        public int WorkingHoursPerDay { get; set; }
        [XmlAttribute]
        public int DaysVacationPerYear { get; set; }

        public Settings() { }

        public void SetDefaults()
        {
            ThemeColor = "dark";
            AutoInsertTime = 0;
            ExportDirectory = @"Export\";
            DurationLunchTime = 1;
            WorkingHoursPerDay = 7;
            DaysVacationPerYear = 30;
        }
    }
}
