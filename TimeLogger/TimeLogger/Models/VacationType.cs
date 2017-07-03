using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TimeLogger.Models
{
    public class VacationType
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }
        [XmlAttribute]
        public bool CountEnabled { get; set; }

        public VacationType() { }
    }
}
