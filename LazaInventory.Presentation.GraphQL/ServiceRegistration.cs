using LazaInventory.Presentation.GraphQL.Queries;

namespace LazaInventory.Presentation.GraphQL;

public static class ServiceRegistration
{
    public static void AddGraphQLLayer(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddQueryType<Query>();
    }
}