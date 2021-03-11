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
        public StationControl(IDoor door, IUsbCharger charger, ILogFile log)
        {
            _door = door;
            _charger = charger;
            charger.ChargeEvent += ChargerHandleEvent;
            _log = log;
        }


        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum ChargingStationState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private ChargingStationState _state;
        private IUsbCharger _charger;
        private ILogFile _log;
        private IDoor _door;
        private IDisplay _display;
        private int _oldId;
        private int _id;
        public double ChargeWatt { get; set; }


        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case ChargingStationState.Available:
                    // Check for ladeforbindelse
                    if (_charger.Connected)
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                        using (var writer = File.AppendText(logFile))
                        {
                             _display.ShowMessage(DateTime.Now + ": Skab låst med RFID: {id}");
                        }

                        Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = ChargingStationState.Locked;
                    }
                    else
                    {
                        _display.ShowMessage("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case ChargingStationState.DoorOpen:
                    // Ignore
                    break;

                case ChargingStationState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", id);
                        }

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = ChargingStationState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
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
