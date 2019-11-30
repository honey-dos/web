using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class InvalidArgumentResult<T> : IDomainResult<T>
    {
        public DomainErrorCode Code => DomainErrorCode.InvalidArgument;
        public T Value => default(T);
        public bool HasError => true;
        public string Message { get; private set; }

        public InvalidArgumentResult(string message)
        {
            Message = message;
        }
    }

    public class InvalidArgumentResult : IDomainResult
    {
        public DomainErrorCode Code => DomainErrorCode.InvalidArgument;
        public bool HasError => true;
        public string Message { get; private set; }

        public InvalidArgumentResult(string message)
        {
            Message = message;
        }
    }
}
