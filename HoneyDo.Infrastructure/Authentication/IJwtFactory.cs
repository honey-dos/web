using HoneyDo.Domain.Entities;

namespace HoneyDo.Infrastructure.Authentication
{
    public interface IJwtFactory
    {
        string GenerateToken(Account account);
    }
}