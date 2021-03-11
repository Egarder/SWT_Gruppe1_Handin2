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
        public LogFile(IRFIDReader reader)
        {
            reader.ScanEvent += ScanEventHandler;
        }

        public void LogDoorLocked(int id)
        {
            throw new NotImplementedException();
        }

        public void LogDoorUnlocked(int id)
        {
            throw new NotImplementedException();
        }

        private void ScanEventHandler(object o, ScanEventArgs e)
        {

            _ = WriteToFile("RFID Card ID:" + e.ID.ToString());
        }

        private void DoorMoveEventHandler(object o, DoorMoveEventArgs e)
        {
            _ = WriteToFile("Door has been opened: " + e.HasOpened.ToString());
        }

        private void DoorLockEventHandler(object o, DoorLockEventArgs e)
        {
            _ = WriteToFile("Door Is locked: " + e.IsLocked.ToString());
        }

        private static async Task WriteToFile(string line)
        {
            await using StreamWriter file = new("LogFile.txt", append: true);
            await file.WriteLineAsync(DateTime.Now.ToShortDateString() + ": " + line);
        }
    }
}
