using System;

namespace ChargingStationClassLib.Models
{
    public interface IRFIDReader
    {
        public int CardID { get; set; }

        event EventHandler<ScanEventArgs> ScanEvent;
    }
}
