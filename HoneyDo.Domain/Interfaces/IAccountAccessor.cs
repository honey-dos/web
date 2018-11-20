using System.Threading.Tasks;
using HoneyDo.Domain.Entities;

namespace HoneyDo.Domain.Interfaces
{
    public interface IAccountAccessor
    {
        Task<Account> GetAccount();
    }
}
