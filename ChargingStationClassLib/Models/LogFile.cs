using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
