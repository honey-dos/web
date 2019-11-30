using HoneyDo.Domain.Enums;

namespace HoneyDo.Domain.Values
{
    public struct DomainError<T> where T : class
    {
        public DomainErrorCode ErrorCode { get; private set; }
        public T Value { get; private set; }
        public bool HasError { get; private set; }
        public string Message { get; private set; }

        public DomainError(T value)
        {
            ErrorCode = default;
            Value = value;
            Message = "";
            HasError = false;
        }

        public DomainError(DomainErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Value = default;
            Message = message;
            HasError = true;
        }
    }

    public struct DomainError
    {
        public DomainErrorCode ErrorCode { get; private set; }
        public bool HasError { get; private set; }
        public string Message { get; private set; }

        public DomainError(DomainErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
            HasError = true;
        }

        public static DomainError NoError() => new DomainError(DomainErrorCode.NoError, "");

        public static DomainError NotFound() => new DomainError(DomainErrorCode.NotFound, "");

        public static DomainError NotAuthorized() => new DomainError(DomainErrorCode.NotAuthorized, "");

        public static DomainError InvalidArgument(string message = "") => 
          new DomainError(DomainErrorCode.InvalidArgument, message);
    }
}
