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
            DoorOpen
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
                string message = "";

                if (_oldId == e.ID)
                {
                    message = "Rfid-kort scannet og godkendt - Skab låses op";
                    _door.UnlockDoor();
                }
                else
                    message = "Rfid-kort scannet - Skab allerede i brug";

                _display.ShowMessage(message);
                _log.WriteToLog(message);
            }

            //state = DooOpen -> writeout:"Please close door"  - ?
        }

        private void DoorClosedHandleEvent(object o, DoorMoveEventArgs e)
        {
            string message = "";

            if (!e.HasOpened)
            {
                message = "Door locked";
                _door.LockDoor();
                _chargeControl.StartCharge();
            }

            else
                message = "Please close door";

            _display.ShowMessage(message);
            _log.WriteToLog(message);
        }


        private void ChargerHandleEvent(object sender, ChargerEventArgs CEA)
        {
            string message = "";

            ChargeWatt = CEA.Current;

            if (ChargeWatt > 0 && ChargeWatt <= 5)
                message = "Phone fully charged";

            else if (ChargeWatt > 500)
                message = "ERROR! Faulty Charger!";

            else
                message = "Charging";

            _display.ShowMessage(message);
            _log.WriteToLog(message);
        }


    }
}
