using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class DoorMoveEventArgs : EventArgs
    {
        public bool HasClosed { get; set; }
    }

    public class Door : IDoor
    {
        public bool Locked { get; set; }
        public bool Closed { get; set; }

        public event EventHandler<DoorMoveEventArgs> DoorMoveEvent;

        public Door()
        {
            Locked = false;
            Closed = true;
        }

        public void OpenDoor()
        {
            if (!Locked && Closed)
            {
                Closed = false;
                OnDoorMoveEvent(new DoorMoveEventArgs { HasClosed = false });
            }
        }

        public void CloseDoor()
        {
            if (!Closed)
            {
                Closed = true;
                OnDoorMoveEvent(new DoorMoveEventArgs { HasClosed = true });
            }
        }

        public void LockDoor()
        {
            if (!Locked && Closed)
            {
                Locked = true;
            }
        }

        public void UnlockDoor()
        {
            if (Locked && Closed)
            {
                Locked = false;
            }
        }

        protected virtual void OnDoorMoveEvent(DoorMoveEventArgs e)
        {
            DoorMoveEvent?.Invoke(this, e);
        }

    }
}
