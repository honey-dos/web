using System;
using HotChocolate;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL
{
    public static class HoneyDosSchema
    {
        public static Action<IObjectTypeDescriptor<T>> Authorize<T>() => configure => configure.Authorize();

        public static ISchema BuildSchema(IServiceProvider services) => SchemaBuilder.New()
                    .AddServices(services)
                    .AddAuthorizeDirectiveType()
                    .AddQueryType<TodoQuery>(Authorize<TodoQuery>())
                    .AddMutationType<TodoMutation>(Authorize<TodoMutation>())
                    /* .AddSubscriptionType<SubscriptionType>() */
                    /* .AddType<TodoType>() */
                    /* .AddType<DroidType>() */
                    /* .AddType<EpisodeType>() */
                    .Create();
    }
}
