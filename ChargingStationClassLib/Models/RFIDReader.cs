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
                try
                {
                    if (cardID >= 0)
                    {
                        if (value != cardID)
                        {
                            OnScanEvent(this, new ScanEventArgs { ID = value }); //Notifies when cardID is set
                            cardID = value;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e+" Invalid card ID, has to be positive");
                    throw;
                }
                
                
            } 
        }

        protected virtual void OnScanEvent(Object o, ScanEventArgs e)
        {
            ScanEvent?.Invoke(this,e);
        }
    }
}
