using System;


namespace ChargingStationClassLib.Models
{
    public class Log : ILog
    {
        public string LogText { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    


}
