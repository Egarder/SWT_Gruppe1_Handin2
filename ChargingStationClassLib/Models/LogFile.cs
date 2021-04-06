using System;
using System.Collections.Generic;

namespace ChargingStationClassLib.Models
{
    public class LogFile : ILogFile
    {
        public LogFile()
        {
            LogList = new List<ILog>();
        }
        public void WriteToLog(string message, DateTime timeStamp)
        {
            LogList.Add(new Log {LogText = message, TimeStamp = timeStamp});
        }

        public List<ILog> LogList { get; set; }
    }
}
