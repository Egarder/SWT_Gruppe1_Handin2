using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class StationControl
    {
        public StationControl(IDoor door, ILogFile log, IRFIDReader rfid, IChargeControl chargeControl, IUsbCharger usbCharger, IDisplay display)
        {
            _door = door;
            _log = log;
            _rfid = rfid;
            _usbCharger = usbCharger;
            _chargeControl = chargeControl;
            _display = display;

            _usbCharger.ChargeEvent += ChargerHandleEvent;
            _rfid.ScanEvent += RFIDDetectedHandleEvent;
            _door.DoorMoveEvent += DoorClosedHandleEvent;

            _state = ChargingStationState.Available;
            _usbCharger.Connected = false;
            _oldId = -1;
        }

        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum ChargingStationState
        {
            Available,
            Locked,
            Opened
        };

        #region fields
        private ILogFile _log;
        private IDoor _door;
        private IDisplay _display;
        private IUsbCharger _usbCharger;
        private IRFIDReader _rfid;
        private IChargeControl _chargeControl;

        private ChargingStationState _state;
        private int _oldId;
        #endregion fields

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
            if (_state == ChargingStationState.Available && _door.Closed)
            {
                message = $"ID: {e.ID} scanned.";
                _oldId = e.ID;

                if (_usbCharger.Connected)
                {
                    _state = ChargingStationState.Locked;
                    _door.LockDoor();
                    _chargeControl.StartCharge();
                    message = "Charging started";
                }
                else
                    message = "Phone not connected";

            }

            else if (_state == ChargingStationState.Locked)
            {
                if (_oldId == e.ID)
                {
                    _usbCharger.Connected = false;
                    _chargeControl.StopCharge();
                    OldId = -1;
                    _door.UnlockDoor();
                    _state = ChargingStationState.Available;
                    message = "ID Scanned and approved - Charging stopped";
                }
                else
                    message = "ID scanned - Closet already in use";
            }

            else if (_state == ChargingStationState.Opened)
                message = "Door not closed";

            else
                message = "Phone not connected";

            _display.ShowMessage(message);
            _log.WriteToLog(message, TimeStamp);
        }
        
        private void DoorClosedHandleEvent(object o, DoorMoveEventArgs door)
        {
            if (door.HasClosed)
            {
                _state = ChargingStationState.Available;
                message = "Door closed";
            }

            else
                _state = ChargingStationState.Opened;

            _display.ShowMessage(message);
            _log.WriteToLog(message, TimeStamp);
        }


        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {
            ChargeWatt = CEA.Current;

            if (ChargeWatt > 0 && ChargeWatt <= 5)
            {
                message = "Phone fully charged";
                _display.ShowMessage(message);
                _log.WriteToLog(message, TimeStamp);
            }

            else if (ChargeWatt > 500)
            {
                message = "ERROR! Faulty Charger!";
                _display.ShowMessage(message);
                _log.WriteToLog(message, TimeStamp);
            }
        }
    }
}
