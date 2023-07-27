using System.ComponentModel;

namespace Wanderland.Tour.Application;

public enum TourState
{
    [Description("Request sumbitted")]
    Submitted=3,
    [Description("Flight reserved")]
    FlightReserved = 4,
    [Description("Return flight reserved")]
    ReturnFlightReserved

}