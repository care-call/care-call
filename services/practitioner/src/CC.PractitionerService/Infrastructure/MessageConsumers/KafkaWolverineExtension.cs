using Wolverine;
using Wolverine.Kafka;

namespace CC.PractitionerService.Infrastructure.MessageConsumers;

public class KafkaWolverineExtension(IConfiguration configuration) : IWolverineExtension
{
    public void Configure(WolverineOptions options)
    {
        options.UseKafka(configuration.GetConnectionString("Kafka")!);
        options.Discovery.CustomizeHandlerDiscovery(x =>
        {
            x.Includes.WithNameSuffix("Consumer");
            x.ToEndpoint("Consume");
            x.ToEndpoint("ConsumeAsync");
        });
        options.ListenToKafkaTopics("appointments");
    }
}
