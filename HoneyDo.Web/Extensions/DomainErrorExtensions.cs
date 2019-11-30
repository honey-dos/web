using HoneyDo.Domain.Values;
using HoneyDo.Web.Models;
using HotChocolate;

namespace HoneyDo.Web.Extensions
{
    public static class DomainErrorExtensions
    {
        public static IError ForGraphQL<T>(this DomainError<T> error) where T : class =>
            ErrorBuilder.New()
            .SetCode(error.ErrorCode.ToString())
            .SetMessage(error.Message)
            .Build();

        public static IError ForGraphQL(this DomainError error) =>
            ErrorBuilder.New()
            .SetCode(error.ErrorCode.ToString())
            .SetMessage(error.Message)
            .Build();

        public static ErrorModel ForRestApi<T>(this DomainError<T> error) where T : class =>
            new ErrorModel { Error = error.ErrorCode.ToString(), Message = error.Message };

        public static ErrorModel ForRestApi(this DomainError error) =>
            new ErrorModel { Error = error.ErrorCode.ToString(), Message = error.Message };
    }
}
