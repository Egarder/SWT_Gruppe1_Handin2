﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public interface ILogFile
    {
        public void WriteToLog(string message, DateTime timeStamp);
    }
}
