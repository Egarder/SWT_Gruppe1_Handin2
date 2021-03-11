using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class LogFile : ILogFile
    {

        public LogFile()
        { }
        public LogFile(string fileName/*IRFIDReader reader, IDoor door*/)
        {
            FileName = fileName;
            //reader.ScanEvent += ScanEventHandler;
            //door.DoorLockEvent += DoorLockEventHandler;
            //door.DoorMoveEvent += DoorMoveEventHandler;
        }

        public async void WriteToLog(string text)
        {
            await WriteToFileTask(text);
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
        public async Task WriteToFileTask(string text)
        {
            await using StreamWriter file = new(FileName, append: true);
            await file.WriteLineAsync(DateTime.Now + ": " + text);
        }

        public string FileName { get; set; } = "LogFile.txt";
    }
}
