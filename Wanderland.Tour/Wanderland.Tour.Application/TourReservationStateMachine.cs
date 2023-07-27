using MassTransit;
using Wanderland.Contracts.Commands;
using Wanderland.Contracts.Events;

namespace Wanderland.Tour.Application;

public class TourReservationStateMachine : MassTransitStateMachine<SagaInstance>
{
    //States
    public State Submitted { get; private set; }
    public State FlightReserved { get; private set; }
    public State ReturnFlightReserved { get; private set; }

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
        InstanceState(x => x.CurrentState, Submitted, FlightReserved, ReturnFlightReserved);

        //what behaviors we have
        Initially(
            When(ReservationSubmittedEvent)
                .Then(context => context.Saga.ReservationDetail = new ReservationDetail(context.Message.CustomerId, context.Message.HotelId, context.Message.RoomNumber, context.Message.FlightId,context.Message.FlightSeat,context.Message.ReturnFlightId,context.Message.ReturnFlightSeat))
                .Then(context=> Console.WriteLine($"Message received with correlationId {context.CorrelationId} and the state is {context.Saga.CurrentState}"))
                .Publish(context=>
                    new ReserveFlightCommand(context.Message.CustomerId,context.Message.TourId,context.Message.FlightId,context.Message.FlightSeat),(context =>{Console.WriteLine("message published");} ))
                .TransitionTo(Submitted));

        During(Submitted,
            When(FlightReservedEvent)
                .Then(x => Console.WriteLine(
                    $"Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                .Publish(context =>
                        new ReserveFlightCommand(context.Saga.ReservationDetail.CustomerId, context.Saga.CorrelationId,
                            context.Saga.ReservationDetail.ReturnFlightId,
                            context.Saga.ReservationDetail.ReturnFlightSeat),
                    (context => { Console.WriteLine("During submitted, ReserveFlightCommand message published"); }))
                .TransitionTo(FlightReserved));
          
        During(FlightReserved,
            When(FlightReservedEvent)
                .Then(x => Console.WriteLine($"During FlightReserved, FlightReservedEvent Message received with correlationId {x.CorrelationId} and the state is {x.Saga.CurrentState}"))
                .TransitionTo(ReturnFlightReserved));


        During(FlightReserved,
            Ignore(ReservationSubmittedEvent));
        
        During(ReturnFlightReserved,
            Ignore(FlightReservedEvent));


        DuringAny(
            When(SagaStateRequestedEvent)
                .Respond(context => new SagaStateResponse
                {
                    State = context.Saga.CurrentState,
                    TourId = context.Saga.CorrelationId
                })
        );

    }


}