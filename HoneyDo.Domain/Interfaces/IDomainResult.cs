using HoneyDo.Domain.Enums;

namespace HoneyDo.Domain.Interfaces
{
    public interface IDomainResult<T>
    {
        DomainResultCode Code { get; }
        T Value { get; }
        bool HasError { get; }
        string Message { get; }
    }

    public interface IDomainResult
    {
        DomainResultCode Code { get; }
        bool HasError { get; }
        string Message { get; }
    }
}
