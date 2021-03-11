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
        
        private int cardID;
        public int CardID
        {
            get { return cardID; }
            set
            {
                if (value != cardID)
                {
                    OnScanEvent(new ScanEventArgs{ ID =value}); //Notifies when cardID is set
                    cardID = value;
                }
            } 
        }

        protected virtual void OnScanEvent(ScanEventArgs e)
        {
            ScanEvent?.Invoke(this,e);
        }
    }
}
