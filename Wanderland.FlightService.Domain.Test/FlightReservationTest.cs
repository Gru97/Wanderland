using FluentAssertions;
using Wanderland.Flight.Domain;
using Xunit;

namespace Wanderland.Flight.Domain.Test
{
    public class FlightReservationTest
    {
        [Fact]
        public void Reserve_Flight_Successfully()
        {
            var origin = new City(Cities.Tehran.Id) ;
            var destination = new City(Cities.Kish.Id);
            var flight = new Domain.Flight(origin, destination, 50);
            var passenger = new Passenger(Persona.JackThePassenger.Id);
            var seatNumber = 12;

            var ticket = flight.ReserveTicket(passenger, seatNumber);

            ticket.Should().NotBeNull();
            ticket.FlightId.Should().Be(flight.Id);
            ticket.Passenger.Should().Be(passenger);
            ticket.SeatNumber.Should().Be(seatNumber);
        }
    }
}