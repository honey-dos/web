using System.Threading.Tasks;
using HoneyDo.Domain.Entities;

namespace HoneyDo.Infrastructure.Providers
{
    public interface IProvider
    {
        string Provider { get; }
        Task<string> GetProviderKey(string accessToken);
    }
}