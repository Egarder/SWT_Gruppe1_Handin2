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
        public StationControl(IDoor door, IUsbCharger charger, ILogFile log, IRFIDReader rfid)
        {
            
            _door = door;
            _charger = charger;
            _log = log;
            _rfid = rfid;

            _charger.ChargeEvent += ChargerHandleEvent;
            _rfid.ScanEvent += RFIDDetectedHandleEvent;

            _state = ChargingStationState.Available;

        }

        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum ChargingStationState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private IUsbCharger _charger;
        private ILogFile _log;
        private IDoor _door;
        private IDisplay _display;
        private IRFIDReader _rfid;
        private int _oldId;
        private int _id;
        private ChargingStationState _state;

        public double ChargeWatt { get; set; }


        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen

        private void RFIDDetectedHandleEvent(Object o, ScanEventArgs e)
        {
            if (_state == ChargingStationState.Available)
            {
                _oldId = e.ID;
                _door.UnlockDoor();
                _display.ShowMessage("ID scannet. Dør låst op");
            }

            else if (_state == ChargingStationState.Locked)
            {
                if (_oldId == e.ID)
                    _display.ShowMessage("");
            }
        }


        private void DoorMovementStateChanged(Object o, DoorMoveEventArgs e)
        {
            if (e.HasOpened)
                _display.ShowMessage("Door opened");

            else if (!e.HasOpened)
                _display.ShowMessage("Door closed");
        }

        private void DoorLockStateChanged(Object o, DoorLockEventArgs e)
        {
            if (e.IsLocked)
                _display.ShowMessage("Door Locked");

            else if (!e.IsLocked)
                _display.ShowMessage("Door Unlocked");
        }

        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {
            ChargeWatt = CEA.Current;
            if (ChargeWatt > 0 && ChargeWatt <= 5)
            {
                _display.ShowMessage("Phone fully charged");
            }
            else if (ChargeWatt > 500)
            {
                _display.ShowMessage("ERROR! Faulty Charger!");
            }
            else
            {
                _display.ShowMessage("Charing");
            }
        }


    }
}
