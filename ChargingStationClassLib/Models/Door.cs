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
            Locked = true;
            Closed = true;
        }

        public void OpenDoor()
        {
            if (!Locked && Closed)
            {
                Closed = false;
                Console.WriteLine("Door opened");
                OnDoorMoveEvent(new DoorMoveEventArgs { HasClosed = false });
            }

            else if (Locked)
                Console.WriteLine("Cant open door - Door locked");

            else
                Console.WriteLine("Door already open");
        }

        public void CloseDoor()
        {
            if (!Closed)
            {
                Closed = true;
                Console.WriteLine("Door closed");
                OnDoorMoveEvent(new DoorMoveEventArgs { HasClosed = true });
            }
            else Console.WriteLine("Door already closed");
        }

        public void LockDoor()
        {
            if (!Locked && Closed)
            {
                Locked = true;
                Console.WriteLine("Door Locked");
            }

            else if (!Locked && !Closed)
                Console.WriteLine("Cant lock door - door not closed");

            else Console.WriteLine("Door already locked");
        }

        public void UnlockDoor()
        {
            if (Locked)
            {
                Locked = false;
                Console.WriteLine("Door unlocked");
            }

            else
                Console.WriteLine("Door already unlocked");


        }

        protected virtual void OnDoorMoveEvent(DoorMoveEventArgs e)
        {
            DoorMoveEvent?.Invoke(this, e);
        }

    }
}
