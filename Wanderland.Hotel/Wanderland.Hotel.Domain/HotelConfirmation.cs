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
        public List<Room> Seats{ get; private set; }
        public int SeatCount{ get; private set; }
        public Hotel(City origin, City destination,int seatCount)
        {
            Origin=origin;
            Destination=destination;
            SeatCount = seatCount;
            InitializeRooms();
        }

        private void InitializeRooms()
        {
            Seats = new List<Room>();
            for (int i = 1; i <= SeatCount; i++)
            {
                Seats.Add(new Room(i));
            }
        }

        public HotelConfirmation ReserveTicket(Passenger passenger, int roomNumber)
        {
            Guard.Against(roomNumber==0 || roomNumber>SeatCount,new DomainException(ErrorMessage.RoomNumberIsInvalid));

            var seat = Seats.Find(e => e.Number == roomNumber);
            if (seat == null)
                throw new DomainException(ErrorMessage.RoomNumberIsInvalid);
           
            if(seat.IsReserved)
                throw new DomainException(ErrorMessage.RoomIsReserved);

            seat.Reserve(); 
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