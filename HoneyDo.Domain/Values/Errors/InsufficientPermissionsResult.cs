using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class InsufficientPermissionsResult<T> : IDomainResult<T>
    {
        public DomainResultCode Code => DomainResultCode.InsufficientPermissions;
        public T Value => default(T);
        public bool HasError => true;
        public string Message { get; private set; }
    }

    public class InsufficientPermissionsResult : IDomainResult
    {
        public DomainResultCode Code => DomainResultCode.InsufficientPermissions;
        public bool HasError => true;
        public string Message { get; private set; }
    }
}
