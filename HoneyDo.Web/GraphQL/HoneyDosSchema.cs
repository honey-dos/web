using System;
using HotChocolate;
using HoneyDo.Web.GraphQL.Types;

namespace HoneyDo.Web.GraphQL
{
    public class HoneyDosSchema
    {
        public static ISchema BuildSchema(IServiceProvider services) => SchemaBuilder.New()
                    .AddServices(services)
                    .AddAuthorizeDirectiveType()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .AddType<AccountType>()
                    .AddType<GroupType>()
                    .AddType<TodoType>()
                    .AddType<TodoCreateFormType>()
                    .AddType<TodoUpdateFormType>()
                    .Create();
    }
}
