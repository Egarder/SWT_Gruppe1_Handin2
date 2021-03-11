using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class StationControl
    {
        public StationControl(IDoor door, ILogFile log, IRFIDReader rfid, IChargeControl chargeControl, IUsbCharger usbCharger)
        {
            _door = door;
            _log = log;
            _rfid = rfid;
            _usbCharger = usbCharger;
            _chargeControl = chargeControl;

            _usbCharger.ChargeEvent += ChargerHandleEvent;
            _rfid.ScanEvent += RFIDDetectedHandleEvent;
            _door.DoorMoveEvent += DoorClosedHandleEvent;

            _state = ChargingStationState.Available;

        }

        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum ChargingStationState
        {
            Available,
            Locked,
            Opened
        };

        // Her mangler flere member variable
        private ILogFile _log;
        private IDoor _door;
        private IDisplay _display;
        private IUsbCharger _usbCharger;
        private IRFIDReader _rfid;
        private IChargeControl _chargeControl;
        private int _oldId;
        private ChargingStationState _state;
        public DateTime TimeStamp { get; set; }
        public ChargingStationState State { get => _state; set => _state = value; }


        private string message = "";
        public double ChargeWatt { get; set; }
        public int OldId
        {
            get { return _oldId; }
            set { _oldId = value; }
        }


        private void RFIDDetectedHandleEvent(Object o, ScanEventArgs e)
        {
            if (_state == ChargingStationState.Available)
            {
                _oldId = e.ID;
                _door.UnlockDoor();
                _display.ShowMessage($"ID: {e.ID} scannet. Dør låst op");
            }

            else if (_state == ChargingStationState.Locked)
            {
                if (_oldId == e.ID)
                {
                    message = "Rfid-kort scannet og godkendt - Skab låses op";
                    _door.UnlockDoor();
                    _state = ChargingStationState.Available;
                }
                else
                    message = "Rfid-kort scannet - Skab allerede i brug";

                _display.ShowMessage(message);
                _log.WriteToLog(message,TimeStamp);
            }

            else
            {
                message = "Please close the door";
                _display.ShowMessage(message);
            }
        }

        private void DoorClosedHandleEvent(object o, DoorMoveEventArgs e)
        {
            if (!e.HasOpened && _state == ChargingStationState.Available && _usbCharger.Connected)
            {
                _door.LockDoor();
                _chargeControl.StartCharge();
                message = "Door locked";
                _state = ChargingStationState.Locked;
            }

            else if (!e.HasOpened && _state == ChargingStationState.Available && !_usbCharger.Connected)
            {
                message = "Please connect phone";
            }

            else if (!e.HasOpened && _state == ChargingStationState.Locked)
            {
                _state = ChargingStationState.Available;
            }

            else
            {
                _state = ChargingStationState.Opened;
                message = "Please close door";
            }

            _display.ShowMessage(message);
            _log.WriteToLog(message, TimeStamp);
        }


        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {
            ChargeWatt = CEA.Current;

            if (ChargeWatt > 0 && ChargeWatt <= 5)
                message = "Phone fully charged";

            else if (ChargeWatt > 500)
                message = "ERROR! Faulty Charger!";

            else
                message = "Charging";

            _display.ShowMessage(message);
            _log.WriteToLog(message, TimeStamp);
        }


    }
}
