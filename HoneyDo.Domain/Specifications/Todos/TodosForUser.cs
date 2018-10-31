using System;
using System.Collections.Generic;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodosForUser : ISpecification<Todo>
    {
        public IEnumerable<Todo> Filter(IEnumerable<Todo> items) => items;
    }
}
