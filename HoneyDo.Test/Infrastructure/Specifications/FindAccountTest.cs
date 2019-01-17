using HoneyDo.Infrastructure.Specifications;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test.Infrastructure.Specifications
{
    public class FindAccountTest
    {
        [Fact]
        public void FindAccountTrue()
        {
            var account = new Account("foobar");
            var findAccount = new FindAccount(account.Id);
            var result = findAccount.BuildExpression().Compile()(account);
            Assert.True(result);
        }
        [Fact]
        public void FindAccountFalse()
        {
            var account = new Account("foobar");
            var fakeAccount = new Account("foobar2");
            var findAccount = new FindAccount(fakeAccount.Id);
            var result = findAccount.BuildExpression().Compile()(account);
            Assert.False(result);
        }
    }
}