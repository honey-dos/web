using System;
using System.Collections.Generic;
using System.Linq;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodoById : ISpecification<Todo>
    {
        private readonly Guid _id;
        public TodoById(Guid id)
        {
            _id = id;
        }
        public IEnumerable<Todo> Filter(IEnumerable<Todo> items) => items.Where(i => i.Id == _id);
    }
}
