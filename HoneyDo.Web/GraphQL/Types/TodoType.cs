using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HoneyDo.Web.Extensions;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL.Types
{
    public class TodoType : ObjectType<Todo>
    {
        protected override void Configure(IObjectTypeDescriptor<Todo> descriptor)
        {
            descriptor.Field("group")
                .Type<GroupType>()
                .Resolver(async ctx => ctx.Parent<Todo>().GroupId.HasValue
                    ? (await ctx.Service<GroupService>().Get(ctx.Parent<Todo>().GroupId.Value)).ForGraphQL(ctx)
                    : null);
            descriptor.Field("assignee")
                .Resolver(ctx => ctx.Parent<Todo>().AssigneeId.HasValue
                    ? ctx.Service<AccountService>().Get(ctx.Parent<Todo>().AssigneeId.Value)
                    : null);
            descriptor.Field("creator")
                .Resolver(ctx => ctx.Service<AccountService>().Get(ctx.Parent<Todo>().CreatorId));
        }
    }
}
