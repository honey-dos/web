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
        public void BuildExpressionTrue()
        {
            var account = new Account("foobar");
            var newTodo = new Todo("test todo", account);
            var todoForUser = new TodosForUser(account);
            var result = todoForUser.BuildExpression().Compile()(newTodo);
            Assert.True(result);
        }
        [Fact]
        public void BuildExpressionFalse()
        {
            var account = new Account("foobar");
            var fakeAccount = new Account("foobar2");
            var newTodo = new Todo("test todo", account);
            var fakeTodo = new Todo("test todo2", fakeAccount);
            var todoForUser = new TodosForUser(account);
            var result = todoForUser.BuildExpression().Compile()(fakeTodo);
            Assert.False(result);
        }
    }
}