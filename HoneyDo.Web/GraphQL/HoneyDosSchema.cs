using System;
using HotChocolate;

namespace HoneyDo.Web.GraphQL
{
    public static class HoneyDosSchema
    {
        public static ISchema BuildSchema(IServiceProvider services) => SchemaBuilder.New()
                    .AddServices(services)
                    .AddQueryType<TodoQuery>()
                    .AddMutationType<TodoMutation>()
                    /* .AddSubscriptionType<SubscriptionType>() */
                    /* .AddType<TodoType>() */
                    /* .AddType<DroidType>() */
                    /* .AddType<EpisodeType>() */
                    .Create();
    }
}
