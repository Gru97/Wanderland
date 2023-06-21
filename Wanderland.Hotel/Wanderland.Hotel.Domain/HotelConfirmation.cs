using FizzWare.NBuilder.Implementation;
using Seedwork.DomainDriven.Core;

namespace Wanderland.Hotel.Domain
{
    public class Passenger: ValueObject
    {
        public Guid Id { get; private set; }
        public Passenger(Guid id)
        {
            Id = id;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }
    }

    public class Hotel: Entity<Guid>
    {
        public City Origin { get; private set; }
        public City Destination { get; private set; }
        public List<Room> Rooms{ get; private set; }
        public int RoomCount{ get; private set; }
        public Hotel(City origin, City destination,int roomCount)
        {
            Origin=origin;
            Destination=destination;
            RoomCount = roomCount;
            InitializeRooms();
        }

        private void InitializeRooms()
        {
            Rooms = new List<Room>();
            for (int i = 1; i <= RoomCount; i++)
            {
                Rooms.Add(new Room(i));
            }
        }

        public HotelConfirmation ReserveTicket(Passenger passenger, int roomNumber)
        {
            Guard.Against(roomNumber==0 || roomNumber>RoomCount,new DomainException(ErrorMessage.RoomNumberIsInvalid));

            var room = Rooms.Find(e => e.Number == roomNumber);
            if (room == null)
                throw new DomainException(ErrorMessage.RoomNumberIsInvalid);
           
            if(room.IsReserved)
                throw new DomainException(ErrorMessage.RoomIsReserved);

            room.Reserve(); 
            return new HotelConfirmation(Id, passenger, roomNumber);
        }

       
       
    }

    public class Room
    {
        public int Number { get; private set; }
        public bool IsReserved{ get; private set; }
        public  Room(int roomNumber)
        {
            Number = roomNumber;
        }
        public void Reserve()
        {
            IsReserved = true;
        }
    }


    public class HotelConfirmation : Entity<Guid>
    {
        public Guid HotelId { get; private set; }
        public Passenger Passenger { get; private set; }
        public int RoomNumber { get; private set; }

        public HotelConfirmation(Guid hotelId, Passenger passenger, int roomNumber)
        {
            HotelId = hotelId;
            Passenger = passenger;
            RoomNumber = roomNumber;
        }
    }
}