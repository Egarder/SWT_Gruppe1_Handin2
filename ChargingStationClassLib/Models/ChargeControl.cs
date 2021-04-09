

namespace ChargingStationClassLib.Models
{
    public class ChargeControl : IChargeControl
    {
        public double ChargeWatt { get; set; }
        private IUsbCharger _charger { get; set; }
        public ChargeControl(IUsbCharger charger)
        {
            _charger = charger;
            charger.ChargeEvent += ChargerHandleEvent;
        }
        public bool IsConnected()
        {
            return _charger.Connected;
        }

        public void StartCharge()
        {
            _charger.StartCharge();
        }

        public void StopCharge()
        {
            _charger.StopCharge();
        }

        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {
            ChargeWatt = CEA.Current;
            if(ChargeWatt >0 && ChargeWatt<=5)
            {
                StopCharge();
            }
            else if(ChargeWatt > 500)
            {
                StopCharge();
            }
        }
    }
}
