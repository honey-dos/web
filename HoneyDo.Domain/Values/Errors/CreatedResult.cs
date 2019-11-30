using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class CreatedResult<T> : IDomainResult<T>
    {
        public DomainErrorCode Code => DomainErrorCode.NoError;
        public T Value { get; private set; }
        public bool HasError => false;
        public string Message => "";

        public CreatedResult(T value)
        {
            Value = value;
        }
    }

    public class CreatedResult : IDomainResult
    {
        public DomainErrorCode Code => DomainErrorCode.NoError;
        public bool HasError => false;
        public string Message => "";
    }
}
