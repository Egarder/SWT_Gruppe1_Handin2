using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class ScanEventArgs : EventArgs
    {
        public int ID { get; set; }
    }

    public class RFIDReader: IRFIDReader
    {
        public event EventHandler<ScanEventArgs> ScanEvent;

        private bool error;

        public bool Error
        {
            get { return error; }
            set { error = value; }
        }

        private int cardID;
        public int CardID
        {
            get { return cardID; }
            set
            {
                if (value >= 0)
                {
                    if (value != cardID)
                    {
                        Error = false;
                        OnScanEvent(new ScanEventArgs { ID = value }); //Notifies when cardID is set
                        cardID = value;
                    }
                }
                else
                    Error = idError();
            } 
        }

        protected virtual void OnScanEvent(Object o, ScanEventArgs e)
        {
            ScanEvent?.Invoke(this,e);
        }

        public bool idError()
        {
            Console.WriteLine(" Invalid card ID, has to be positive");
            return true;
        }
    }
}
