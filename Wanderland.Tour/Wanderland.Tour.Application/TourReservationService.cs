using System.ComponentModel;
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
        private IRequestClient<SagaStateRequestedEvent> _requestClient;

        public TourReservationService(IPublishEndpoint publishEndpoint, IRequestClient<SagaStateRequestedEvent> requestClient)
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

            var tourId = Guid.NewGuid();
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

        public async Task<string> GetState(string id, CancellationToken token)
        {
            try
            {
                var result = await _requestClient.GetResponse<SagaStateResponse>
                    (new SagaStateRequestedEvent { TourId = Guid.Parse(id) }, token);

                var tourState = (TourState)result.Message.State;
                return tourState.GetDescription();
            }
            catch (RequestTimeoutException e)
            {
                throw new ApplicationException("Error while processing your request. The server didn't respond.");
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
        public Event<SagaStateRequestedEvent> SagaStateRequestedEvent { get; private set; }


        public TourReservationStateMachine()
        {
            //how to correlate the event to an instance.
            Event(() => ReservationSubmittedEvent, x => x.CorrelateById(context => context.Message.TourId));
            Event(() => FlightReservedEvent, x => x.CorrelateById(context => context.Message.TourId));
            Event(() => SagaStateRequestedEvent, x => x.CorrelateById(context => context.Message.TourId));

            //what states we have
            // 0 - None, 1 - Initial, 2 - Final
            InstanceState(x => x.CurrentState, Submitted, FlightReserved);

            //what behaviors we have
            Initially(
                When(ReservationSubmittedEvent)
                    .Then(x => x.Saga.ReservationDetail = new ReservationDetail(x.Message.CustomerId, x.Message.HotelId, x.Message.RoomNumber, x.Message.FlightId,x.Message.FlightSeat,x.Message.ReturnFlightId,x.Message.ReturnFlightSeat))
                    .Then(x=> Console.WriteLine($"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                    .Publish(context=>
                        new ReserveFlightCommand(context.Message.CustomerId,context.Message.TourId,context.Message.FlightId,context.Message.FlightSeat),(context =>{Console.WriteLine("message published");} ))
                    .TransitionTo(Submitted));

            DuringAny(
                When(SagaStateRequestedEvent)
                    .Respond(context => new SagaStateResponse
                    {
                        State =  context.Saga.CurrentState ,TourId = context.Saga.CorrelationId
                    })
            );

            During(Submitted,
                When(FlightReservedEvent)
                    .Then(x => Console.WriteLine($"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
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

    public class SagaStateRequestedEvent
    {
        public Guid TourId { get; set; }
    }
    public class SagaStateResponse
    {
        public int State { get; set; }
        public Guid TourId { get; set; }
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
        [Description("Request sumbitted")]
        Submitted=3,
        [Description("Flight reserved")]
        FlightReserved = 4

    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var descriptionAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(false)
                .SingleOrDefault(attr => attr.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute;

            // return description
            return descriptionAttribute?.Description ?? "";
        }
    }
}

