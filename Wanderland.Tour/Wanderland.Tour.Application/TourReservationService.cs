using MassTransit;
using Wanderland.Tour.Application.Commands;

namespace Wanderland.Tour.Application
{
    //https://www.youtube.com/watch?v=Vwfngk0YhLs
    public class TourReservationService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private IRequestClient<SagaState> _requestClient;

        public TourReservationService(IPublishEndpoint publishEndpoint, IRequestClient<SagaState> requestClient)
        {
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
        }

        public async Task<Guid> Reserve(ReserveTourCommand command)
        {
            //var tour = new Tour { };

            //// Save the order to the database
            //await _dbContext.Tours.AddAsync(tour, cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);

            var tourId = Guid.Parse("008f0d2e-c904-4f5d-98a9-0153bca459e6");
            await _publishEndpoint.Publish(new ReservationSubmittedEvent
            {
                FlightId = command.ArrivalFlightId,
                FlightSeat = command.ArrivalFlightSeat,
                CustomerId = command.CustomerId,
                ReturnFlightId = command.DepartureFlightId,
                ReturnFlightSeat = command.DepartureFlightSeat,
                HotelId = command.HotelId,
                RoomNumber = command.RoomNumber,
                TourId = tourId
            });

            return tourId;
        }

        public async Task<string> GetState(string id)
        {
            var result= await _requestClient.GetResponse<SagaState>(new {CorrelationId= id });

            return result.Message.CurrentState.ToString();
        }
    }

    public class TourReservationStateMachine : MassTransitStateMachine<SagaState>
    {
        //States
        public State Submitted { get; private set; }
        public State FlightReserved { get; private set; }

        //Events
        public Event<ReservationSubmittedEvent> ReservationSubmittedEvent { get; private set; }
        public Event<FlightReservedEvent> FlightReservedEvent { get; private set; }


        public TourReservationStateMachine()
        {
            //how to correlate the event to an instance.
            Event(() => ReservationSubmittedEvent, x => x.CorrelateById(context => context.Message.TourId));
            Event(() => FlightReservedEvent, x => x.CorrelateById(context => context.Message.TourId));

            //what states we have
            InstanceState(x => x.CurrentState, Submitted, FlightReserved);

            //what behaviours we have
            Initially(
                When(ReservationSubmittedEvent)
                    .Then(x => x.Saga.CurrentState = (int)TourState.Submitted)
                    .Then(x=> Console.WriteLine($"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                    .Respond(context => new SagaState { CurrentState  = context.Instance.CurrentState})
                    .TransitionTo(Submitted));

            During(Submitted,
                When(FlightReservedEvent)
                    .Then(x => x.Saga.CurrentState = (int)TourState.FlightReserved)
                    .TransitionTo(FlightReserved));

            During(FlightReserved,
                Ignore(ReservationSubmittedEvent));
        }


    }

    public class SagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }

    }

    public enum TourState
    {
        Undefined=0,
        Submitted=1,
        FlightReserved=2

    }
}

