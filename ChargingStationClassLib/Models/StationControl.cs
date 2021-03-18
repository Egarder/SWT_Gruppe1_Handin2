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
            if (_state == ChargingStationState.Available)
            {
                message = $"ID: {e.ID} scanned. Please Open Door";
                _oldId = e.ID;
                _door.UnlockDoor();
                _display.ShowMessage(message);
            }

            else if (_state == ChargingStationState.Locked)
            {
                if (_oldId == e.ID)
                {
                    _usbCharger.StopCharge();
                    message = "ID Scanned and approved";
                    _door.UnlockDoor();
                    _state = ChargingStationState.Available;
                }
                else
                    message = "ID scanned - Closet already in use";

                _display.ShowMessage(message);
                _log.WriteToLog(message,TimeStamp);
            }

            else
            {
                message = "Please close the door";
                _display.ShowMessage(message);
            }
        }

        private void DoorClosedHandleEvent(object o, DoorMoveEventArgs door)
        {
            if (_oldId < 0)
            {
                message = "Please scan card";
            }

            else
            {
                switch (door.HasClosed)
                {
                    case true:

                        if (_state == ChargingStationState.Opened)
                        {
                            if (_usbCharger.Connected)
                            {
                                _door.LockDoor();
                                _chargeControl.StartCharge();
                                _state = ChargingStationState.Locked;
                                message = "Charging started";
                            }

                            else
                            {
                                message = "Please connect phone";
                            }
                        }

                        break;


                    case false:
                        _state = ChargingStationState.Opened;
                        message = "Door Opened. Please Connect Phone";
                        break;
                }
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
