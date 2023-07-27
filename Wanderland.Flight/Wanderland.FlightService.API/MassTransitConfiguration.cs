using MassTransit;
using Wanderland.Flight.API.Consumers;
using Wanderland.Flight.API.Controllers;

namespace Wanderland.Flight.API
{
    public static class MassTransitConfiguration
    {
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ReserveFlightCommandHandler>();
                x.AddConsumer<CancelFlightReservationCommandHandler>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });

        }
    }
}
