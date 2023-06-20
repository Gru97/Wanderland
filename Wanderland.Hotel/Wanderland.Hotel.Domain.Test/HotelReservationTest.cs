using FluentAssertions;
using Xunit;

namespace Wanderland.Hotel.Domain.Test
{
    public class HotelReservationTest
    {
        [Fact]
        public void Reserve_Hotel_Successfully()
        {
            var origin = new City(Cities.Tehran.Id) ;
            var destination = new City(Cities.Kish.Id);
            var Hotel = new Hotel(origin, destination, 50);
            var passenger = new Passenger(Persona.JackThePassenger.Id);
            var seatNumber = 12;

            var ticket = Hotel.ReserveTicket(passenger, seatNumber);

            ticket.Should().NotBeNull();
            ticket.HotelId.Should().Be(Hotel.Id);
            ticket.Passenger.Should().Be(passenger);
            ticket.RoomNumber.Should().Be(seatNumber);
        }
    }
}