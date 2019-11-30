namespace HoneyDo.Domain.Enums
{
    public enum DomainResultCode
    {
        Success = 0,
        Created = 1,
        Deleted = 2,
        NotFound = 3,
        Unauthorized = 4,
        InvalidArgument = 5,
        InsufficientPermissions = 6
    }
}
