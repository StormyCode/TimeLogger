using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.Models
{
    public class VacationLog
    {
        public VacationLog() { }

        public VacationType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
