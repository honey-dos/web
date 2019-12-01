using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class SuccessResult<T> : IDomainResult<T>
    {
        public DomainResultCode Code => DomainResultCode.Success;
        public T Value { get; private set; }
        public bool HasError => false;
        public string Message => "";

        public SuccessResult(T value)
        {
            Value = value;
        }
    }

    public class SuccessResult : IDomainResult
    {
        public DomainResultCode Code => DomainResultCode.Success;
        public bool HasError => false;
        public string Message => "";
    }
}
