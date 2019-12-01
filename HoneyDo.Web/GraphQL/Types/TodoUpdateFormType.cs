using HoneyDo.Web.Models;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL.Types
{
    public class TodoUpdateFormType : InputObjectType<TodoUpdateForm>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TodoUpdateForm> descriptor)
        {
            descriptor.Field(t => t.RemoveGroup).Type<BooleanType>();
            descriptor.Field(t => t.RemoveDueDate).Type<BooleanType>();
            descriptor.Field(t => t.RemoveAssignee).Type<BooleanType>();
        }
    }
}
