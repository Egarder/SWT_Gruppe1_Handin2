using System;

namespace ChargingStationClassLib.Models
{
    public interface IChargeControl
    {
        bool IsConnected();
        void StartCharge();
        void StopCharge();
    }
}