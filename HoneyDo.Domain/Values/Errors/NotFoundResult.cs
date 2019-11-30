using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Values.Errors
{
    public class NotFoundResult<T> : IDomainResult<T>
    {
        public DomainResultCode Code => DomainResultCode.NotFound;
        public T Value => default(T);
        public bool HasError => true;
        public string Message { get; private set; }

        public NotFoundResult(string message)
        {
            Message = message;
        }
    }

    public class NotFoundResult : IDomainResult
    {
        public DomainResultCode Code => DomainResultCode.NotFound;
        public bool HasError => true;
        public string Message { get; private set; }

        public NotFoundResult(string message)
        {
            Message = message;
        }
    }
}
