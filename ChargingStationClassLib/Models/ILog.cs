using System;

namespace ChargingStationClassLib.Models
{
    public interface ILog
    {
        public string LogText { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
