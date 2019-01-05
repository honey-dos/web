using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Specifications.Todos;
using Xunit;

namespace HoneyDo.Test
{
    public class TodosForUserTest
    {
        [Fact]
        public void BuildExpression()
        {
            var account = new Account("foobar");
            var newTodo = new Todo("test todo", account);
            Expression<Func<Todo, bool>> expr = todo => todo.OwnerId == account.Id;
            var todoForUser = new TodosForUser(account);
            var result = todoForUser.BuildExpression().Compile()(newTodo);
            Assert.True(result);
        }
    }
}