using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class UnauthorizedResult<T> : IDomainResult<T>
    {
        public DomainResultCode Code => DomainResultCode.Unauthorized;
        public T Value => default(T);
        public bool HasError => true;
        public string Message { get; private set; }
    }

    public class UnauthorizedResult : IDomainResult
    {
        public DomainResultCode Code => DomainResultCode.Unauthorized;
        public bool HasError => true;
        public string Message { get; private set; }
    }
}
