using MassTransit;
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
}

