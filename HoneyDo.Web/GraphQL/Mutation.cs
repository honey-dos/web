using HotChocolate.Types;
using HoneyDo.Web.GraphQL.Resolvers;

namespace HoneyDo.Web.GraphQL
{
    public class Mutation : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            /* descriptor.Field<AccountResolver>(a => a.GetAccount()).Authorize(); */

            descriptor.Field<TodoResolver>(t => t.CreateTodo(default, default)).Authorize();
            descriptor.Field<TodoResolver>(t => t.UpdateTodo(default, default, default)).Authorize();
            descriptor.Field<TodoResolver>(t => t.DeleteTodo(default, default)).Authorize();

            descriptor.Field<GroupResolver>(t => t.CreateGroup(default, default)).Authorize();
            descriptor.Field<GroupResolver>(t => t.UpdateGroup(default, default, default)).Authorize();
            descriptor.Field<GroupResolver>(t => t.DeleteGroup(default, default)).Authorize();
            descriptor.Field<GroupResolver>(t => t.AddAccounts(default, default, default)).Authorize();
            descriptor.Field<GroupResolver>(t => t.RemoveAccounts(default, default, default)).Authorize();
        }
    }
}
