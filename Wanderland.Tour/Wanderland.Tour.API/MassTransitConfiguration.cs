using MassTransit;
using Wanderland.Tour.Application;
using Wanderland.Tour.Application.Commands;

namespace Wanderland.Tour.API
{
    public static class MassTransitConfiguration
    {
        public static void ConfigureMassTransit(this IServiceCollection  services)
        {
            services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<TourReservationStateMachine, SagaInstance>()
                    .InMemoryRepository();

                x.AddRequestClient<SagaStateRequestedEvent>();


                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                    cfg.AutoStart=true;
                });
            });

        }
    }
}
