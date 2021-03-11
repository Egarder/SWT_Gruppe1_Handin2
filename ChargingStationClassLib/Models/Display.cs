using System;

namespace ChargingStationClassLib.Models
{
    public class Display : IDisplay
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine($"Display: {message}");
        }
    }
}