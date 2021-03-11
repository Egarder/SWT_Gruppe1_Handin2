using System;
using ChargingStationClassLib.Models;

namespace ChargingStationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes

            IDoor _door = new Door();
            IRFIDReader _rfidReader = new RFIDReader();


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
                        finish = true;
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
