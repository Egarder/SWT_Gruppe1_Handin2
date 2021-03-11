using System;

namespace ChargingStationClassLib.Models
{
    public class ChargerEventArgs : EventArgs
    {
        // Value in mA (milliAmpere)
        public double Current { set; get; }
    }

    public interface IUsbCharger
    {
        // Event triggered on new current value
        //event EventHandler<CurrentEventArgs> CurrentValueEvent;
        event EventHandler<ChargerEventArgs> ChargeEvent;

        // Direct access to the current current value
        double CurrentValue { get; }

        // Require connection status of the phone
        bool Connected { get; set; }

        // Start charging
        void StartCharge();
        // Stop charging
        void StopCharge();
    }
}