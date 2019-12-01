using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Values;
using HoneyDo.Web.Models;
using HotChocolate;
using Microsoft.AspNetCore.Mvc;

namespace HoneyDo.Web.Extensions
{
    public static class DomainErrorExtensions
    {
        public static IError ForGraphQL<T>(this DomainError<T> error) where T : class =>
            ErrorBuilder.New()
            .SetCode(error.ErrorCode.ToString())
            .SetMessage(error.Message)
            .Build();

        public static IError ForGraphQL<T>(this IDomainResult<T> error) where T : class =>
            ErrorBuilder.New()
            .SetCode(error.Code.ToString())
            .SetMessage(error.Message)
            .Build();

        public static IError ForGraphQL(this DomainError error) =>
            ErrorBuilder.New()
            .SetCode(error.ErrorCode.ToString())
            .SetMessage(error.Message)
            .Build();

        public static ErrorModelB ForRestApi<T>(this DomainError<T> error) where T : class =>
            new ErrorModelB { Error = error.ErrorCode.ToString(), Message = error.Message };

        public static ErrorModelB ForRestApi(this DomainError error) =>
            new ErrorModelB { Error = error.ErrorCode.ToString(), Message = error.Message };

        public static ActionResult ForRestApi<T>(this IDomainResult<T> result) =>
            new ErrorModel<T>(result).BuildActionResult();

        public static ActionResult ForRestApi(this IDomainResult result) =>
            new ErrorModel(result).BuildActionResult();
    }
}
