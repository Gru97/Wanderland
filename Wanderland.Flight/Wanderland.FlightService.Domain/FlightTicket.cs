using FizzWare.NBuilder.Implementation;
using Seedwork.DomainDriven.Core;

namespace Wanderland.Flight.Domain
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

    public class Flight: Entity<Guid>
    {
        public City Origin { get; private set; }
        public City Destination { get; private set; }
        public List<Seat> Seats{ get; private set; }
        public int SeatCount{ get; private set; }
        public Flight(City origin, City destination,int seatCount)
        {
            Origin=origin;
            Destination=destination;
            SeatCount = seatCount;
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            Seats = new List<Seat>();
            for (int i = 1; i <= SeatCount; i++)
            {
                Seats.Add(new Seat(i));
            }
        }

        public FlightTicket ReserveTicket(Passenger passenger, int seatNumber)
        {
            Guard.Against(seatNumber==0 || seatNumber>SeatCount,new DomainException(ErrorMessage.SeatNumberIsInvalid));

            var seat = Seats.Find(e => e.Number == seatNumber);
            if (seat == null)
                throw new DomainException(ErrorMessage.SeatNumberIsInvalid);
           
            if(seat.IsReserved)
                throw new DomainException(ErrorMessage.SeatIsReserved);

            seat.Reserve(); 
            return new FlightTicket(Id, passenger, seatNumber);
        }

       
       
    }

    public class Seat
    {
        public int Number { get; private set; }
        public bool IsReserved{ get; private set; }
        public  Seat(int seatNumber)
        {
            Number = seatNumber;
        }
        public void Reserve()
        {
            IsReserved = true;
        }
    }


    public class FlightTicket : Entity<Guid>
    {
        public Guid FlightId { get; private set; }
        public Passenger Passenger { get; private set; }
        public int SeatNumber { get; private set; }

        public FlightTicket(Guid flightId, Passenger passenger, int seatNumber)
        {
            FlightId = flightId;
            Passenger = passenger;
            SeatNumber = seatNumber;
        }
    }
}