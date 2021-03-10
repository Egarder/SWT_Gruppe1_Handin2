using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public interface IRFIDReader
    {
        public int CardID { get; set; }

        event EventHandler<ScanEventArgs> ScanEvent;
    }
}
