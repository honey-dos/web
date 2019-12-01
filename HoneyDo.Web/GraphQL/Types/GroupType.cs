using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL.Types
{
    public class GroupType : ObjectType<Group>
    {
        protected override void Configure(IObjectTypeDescriptor<Group> descriptor)
        {
            descriptor.Field(a => a.Tasks)
                .Resolver(ctx => ctx.Service<TodoService>().Get(ctx.Parent<Group>()));
                // .UsePaging<TodoType>();
            descriptor.Field(a => a.Accounts)
                .Resolver(ctx => ctx.Service<AccountService>().Get(ctx.Parent<Group>()));
                // .UsePaging<AccountType>();
        }
    }
}
