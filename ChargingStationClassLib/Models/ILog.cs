using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace ChargingStationClassLib.Models
{
    public interface ILog
    {
        public string LogText { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
