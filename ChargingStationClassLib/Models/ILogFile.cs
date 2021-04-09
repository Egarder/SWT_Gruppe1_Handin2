using System;

namespace ChargingStationClassLib.Models
{
    public interface ILogFile
    {
        public void WriteToLog(string message, DateTime timeStamp);
    }
}
