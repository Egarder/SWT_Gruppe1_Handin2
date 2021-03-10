using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    interface IDoor
    {
        public event EventHandler<DoorLockEventArgs> DoorLockEvent;

        public event EventHandler<DoorMoveEventArgs> DoorMoveEvent;

        public void OpenDoor();
        public void CloseDoor();
        public void LockDoor();
        public void UnlockDoor();
    }
}
