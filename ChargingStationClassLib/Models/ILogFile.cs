﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace ChargingStationClassLib.Models
{
    public interface ILogFile
    {
        public void WriteToLog(string text);

        public string FileName { get; set; }
    }
}
