using HoneyDo.Domain.Enums;

namespace HoneyDo.Domain.Interfaces
{
    public interface IDomainResult<T>
    {
        DomainErrorCode Code { get; }
        T Value { get; }
        bool HasError { get; }
        string Message { get; }
    }

    public interface IDomainResult
    {
        DomainErrorCode Code { get; }
        bool HasError { get; }
        string Message { get; }
    }
}
