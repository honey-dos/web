
using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Specifications.Todos;
using Xunit;

namespace HoneyDo.Test.Domain.Specifications
{
    public class TodoByIdTest
    {
        [Fact]
        public void BuildExpressionTrue()
        {
            var account = new Account("foobar");
            var newTodo = new Todo("test todo",account);
            var todoById = new TodoById(newTodo.Id);
            var result = todoById.BuildExpression().Compile()(newTodo);
            Assert.True(result);
        }
        [Fact]
        public void BuildExpressionFalse()
        {
            var account = new Account("foobar");
            var newTodo = new Todo("test todo",account);
            var fakeTodo = new Todo("test todo 2",account);
            var todoById = new TodoById(fakeTodo.Id);
            var result = todoById.BuildExpression().Compile()(newTodo);
            Assert.False(result);
        }
    }
}