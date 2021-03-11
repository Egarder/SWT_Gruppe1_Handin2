using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class LogFile : ILogFile
    {
        public LogFile(string fileName/*IRFIDReader reader, IDoor door*/)
        {
            FileName = fileName;
            //reader.ScanEvent += ScanEventHandler;
            //door.DoorLockEvent += DoorLockEventHandler;
            //door.DoorMoveEvent += DoorMoveEventHandler;
        }

        //private void ScanEventHandler(object o, ScanEventArgs e)
        //{
        //    _ = WriteToFile("RFID Card ID:" + e.ID.ToString());
        //}

        //private void DoorMoveEventHandler(object o, DoorMoveEventArgs e)
        //{
        //    _ = WriteToFile("Door has been opened: " + e.HasOpened.ToString());
        //}

        //private void DoorLockEventHandler(object o, DoorLockEventArgs e)
        //{
        //    _ = WriteToFile("Door Is locked: " + e.IsLocked.ToString());
        //}
        public async Task WriteToFile(string text)
        {
            await using StreamWriter file = new(FileName, append: true);
            await file.WriteLineAsync(DateTime.Now.ToShortDateString() + ": " + text);
        }

        public string FileName { get; set; }
    }
}
