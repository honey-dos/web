using HoneyDo.Web.Models;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL.Types
{
    public class TodoCreateFormType : InputObjectType<TodoCreateForm>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TodoCreateForm> descriptor)
        {
            descriptor.Field(t => t.Name).Type<NonNullType<StringType>>();
        }
    }
}
