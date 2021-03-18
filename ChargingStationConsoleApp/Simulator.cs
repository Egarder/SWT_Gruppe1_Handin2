﻿using System;
using ChargingStationClassLib.Models;

namespace ChargingStationConsoleApp
{
    class Simulator
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes

            IDoor door = new Door();
            IRFIDReader rfidReader = new RFIDReader();
            ILogFile logFile = new LogFile();
            IUsbCharger usbCharger = new UsbCharger();
            IChargeControl chargeControl = new ChargeControl(usbCharger);
            IDisplay display = new Display();

            StationControl stationController = new StationControl(door, logFile, rfidReader, chargeControl, usbCharger,display);

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast (E)nd, (O)pen, (C)lose, (R)ead Card, Connect (P)hone: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        System.Environment.Exit(1);
                        break;

                    case 'O':
                        door.OpenDoor();
                        break;

                    case 'C':
                        door.CloseDoor();
                        break;

                    case 'R':
                        rfidReader.CardID = int.Parse(Console.ReadLine());
                        break;

                    case 'P':
                        if (door.Closed)
                            Console.WriteLine("Cant connect phone when door is closed");
                        else
                        {
                            usbCharger.Connected = true;
                            Console.WriteLine("Phone connected");
                        }

                        break;
                    default:
                        break;
                }

            } while (!finish);
        }
    }
}
