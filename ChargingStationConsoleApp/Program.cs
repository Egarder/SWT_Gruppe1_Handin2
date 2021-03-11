using System;
using ChargingStationClassLib.Models;

namespace ChargingStationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes

            string filename = "savefile";

            IDoor _door = new Door();
            IRFIDReader _rfidReader = new RFIDReader();
            ILogFile logFile = new LogFile(filename);
            IUsbCharger usbCharger = new UsbCharger();
            IChargeControl chargeControl = new ChargeControl(usbCharger);


            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        
                        break;

                    case 'O':
                        _door.OpenDoor();
                        break;

                    case 'C':
                        _door.CloseDoor();
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }
}
