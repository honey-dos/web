using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL.Types
{
    public class AccountType : ObjectType<Account>
    {
        protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
        {
            descriptor.Field(a => a.Tasks)
                .Resolver(ctx => ctx.Service<TodoService>().Get());
                // .UsePaging<TodoType>();
            descriptor.Field(a => a.Groups)
                .Resolver(ctx => ctx.Service<GroupService>().Get());
                // .UsePaging<TodoType>();
        }
    }
}
