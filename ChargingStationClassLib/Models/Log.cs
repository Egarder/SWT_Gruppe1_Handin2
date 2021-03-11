using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class Log : ILog
    {
        public string LogText { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    


}
