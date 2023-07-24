using MassTransit;
using Wanderland.Contracts.Commands;
using Wanderland.Contracts.Events;
using Wanderland.Tour.Application.Commands;

namespace Wanderland.Tour.Application
{
    //https://www.youtube.com/watch?v=Vwfngk0YhLs
    public class TourReservationService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private IRequestClient<SagaInstance> _requestClient;

        public TourReservationService(IPublishEndpoint publishEndpoint, IRequestClient<SagaInstance> requestClient)
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
                FlightId = command.FlightId,
                FlightSeat = command.FlightSeat,
                CustomerId = command.CustomerId,
                ReturnFlightId = command.ReturnFlightId,
                ReturnFlightSeat = command.ReturnFlightSeat,
                HotelId = command.HotelId,
                RoomNumber = command.RoomNumber,
                TourId = tourId
            });

            return tourId;
        }

        public async Task<string> GetState(string id)
        {
            try
            {
                var result = await _requestClient.GetResponse<SagaInstance>(new { CorrelationId = id });

                return result.Message.CurrentState.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }

    public class TourReservationStateMachine : MassTransitStateMachine<SagaInstance>
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

            //what behaviors we have
            Initially(
                When(ReservationSubmittedEvent)
                    .Then(x => x.Saga.CurrentState = (int)TourState.Submitted)
                    .Then(x => x.Saga.ReservationDetail = new ReservationDetail(x.Message.CustomerId, x.Message.HotelId, x.Message.RoomNumber, x.Message.FlightId,x.Message.FlightSeat,x.Message.ReturnFlightId,x.Message.ReturnFlightSeat))
                    .Then(x=> Console.WriteLine($"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                    . (context => new SagaInstance { CurrentState  = context.Saga.CurrentState})
                    .Publish(context=>
                        new ReserveFlightCommand(context.Message.CustomerId,context.Message.TourId,context.Message.FlightId,context.Message.FlightSeat),(context =>{Console.WriteLine("message published");} ))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(FlightReservedEvent)
                    .Then(x => x.Saga.CurrentState = (int)TourState.FlightReserved)
                    .Then(x => Console.WriteLine($"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                    .Respond(context => new SagaInstance { CurrentState = context.Instance.CurrentState })
                    .Publish(context =>
                        new ReserveFlightCommand(context.Saga.ReservationDetail.CustomerId, context.Saga.CorrelationId, context.Saga.ReservationDetail.ReturnFlightId, context.Saga.ReservationDetail.ReturnFlightSeat), (context => { Console.WriteLine("message published"); }))
                    .TransitionTo(FlightReserved));

            During(FlightReserved,
                Ignore(ReservationSubmittedEvent));

        }


    }

    public class SagaInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public ReservationDetail ReservationDetail { get; set; } 

    }

    public class ReservationDetail
    {
        public Guid CustomerId { get; set; }
        public Guid HotelId { get; set; }
        public int RoomNumber { get; set; }
        public Guid FlightId { get; set; }
        public int FlightSeat { get; set; }
        public Guid ReturnFlightId { get; set; }
        public int ReturnFlightSeat { get; set; }

        public ReservationDetail()
        {
            
        }
        public ReservationDetail(Guid customerId, Guid hotelId, int roomNumber, Guid flightId, int flightSeat, Guid returnFlightId, int returnFlightSeat)
        {
            CustomerId = customerId;
            HotelId = hotelId;
            RoomNumber = roomNumber;
            FlightId = flightId;
            FlightSeat = flightSeat;
            ReturnFlightId = returnFlightId;
            ReturnFlightSeat = returnFlightSeat;
        }
    }

    public enum TourState
    {
        Undefined=0,
        Submitted=1,
        FlightReserved=2

    }
}

