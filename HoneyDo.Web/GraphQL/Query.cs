using HotChocolate.Types;
using HoneyDo.Web.GraphQL.Resolvers;

namespace HoneyDo.Web.GraphQL
{
    public class Query : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<AccountResolver>(a => a.Account()).Authorize();
            descriptor.Field<AccountResolver>(a => a.Accounts(default)).Authorize();

            descriptor.Field<TodoResolver>(t => t.Todo(default)).Authorize();
            descriptor.Field<TodoResolver>(t => t.Todos()).Authorize();

            descriptor.Field<GroupResolver>(t => t.Groups()).Authorize();
            descriptor.Field<GroupResolver>(t => t.Group(default)).Authorize();
        }
    }
}
