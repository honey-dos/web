using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class DeletedResult<T> : IDomainResult<T>
    {
        public DomainResultCode Code => DomainResultCode.Deleted;
        public T Value { get; private set; }
        public bool HasError => false;
        public string Message => "";
    }

    public class DeletedResult : IDomainResult
    {
        public DomainResultCode Code => DomainResultCode.Deleted;
        public bool HasError => false;
        public string Message => "";
    }
}
