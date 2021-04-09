using System;


namespace ChargingStationClassLib.Models
{
    public interface IDoor
    {
        public event EventHandler<DoorMoveEventArgs> DoorMoveEvent;

        public void OpenDoor();

        public void CloseDoor();

        public void LockDoor();

        public void UnlockDoor();

        public bool Locked { get; set; }
        public bool Closed { get; set; }
    }
}
