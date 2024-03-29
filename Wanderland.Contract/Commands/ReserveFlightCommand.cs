﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanderland.Contract.Commands
{
    public record ReserveFlightCommand
    {
        public Guid CustomerId { get; set; }
        public Guid TourId { get; set; }
        public Guid FlightId { get; set; }
        public int FlightSeat { get; set; }

        public ReserveFlightCommand(Guid customerId, Guid tourId, Guid flightId, int flightSeat)
        {
            CustomerId = customerId;
            TourId = tourId;
            FlightId = flightId;
            FlightSeat = flightSeat;
        }
    }
}
