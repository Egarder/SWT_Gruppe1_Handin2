using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class ChargeControl : IChargeControl
    {
        public ChargeControl(IUsbCharger charger)
        {
            charger.ChargeEvent += ChargerHandleEvent;
        }
        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void StartCharge()
        {
            throw new NotImplementedException();
        }

        public void StopCharge()
        {
            throw new NotImplementedException();
        }

        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {

        }
    }
}
