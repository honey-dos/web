using System;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL
{

    public class TodoType : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("Todo");

            descriptor.Field("id")
                .Type<NonNullType<IdType>>();

            descriptor.Field("name")
                .Type<StringType>();

            /* descriptor.Field("height") */
            /*     .Type<FloatType>() */
            /*     .Argument("unit", a => a.Type<EnumType<Unit>>()); */
        }
    }

    public class Query
    {
        public string Todo()
        {
            /* return new Todo("Get graphql to work!", new Account("tyler", "snave")); */
            return "Hello World!";
        }
    }

    public class QueryType : ObjectType<Query>
    {

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            /* descriptor.Field(t => t.GetTodo()) */
            /*     .Type<TodoType>(); */
            /* .Argument("episode", a => a.DefaultValue(Episode.NewHope)); */
        }
    }
}
