using HoneyDo.Domain.Entities;

namespace HoneyDo.Domain.Interfaces
{
    public interface IAccountAccessor
    {
        Account Account { get; }
    }
}