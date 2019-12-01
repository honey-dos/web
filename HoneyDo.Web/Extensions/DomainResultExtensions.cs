using System;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Web.Models;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Mvc;

namespace HoneyDo.Web.Extensions
{
    public static class DomainResultExtensions
    {
        public static T ForGraphQL<T>(this IDomainResult<T> result, IResolverContext ctx) where T : class
        {
            if (result.HasError)
            {
                var error = ErrorBuilder.New()
                  .SetCode(result.Code.ToString())
                  .SetMessage(result.Message)
                  .Build();
                ctx.ReportError(error);
                return default(T);
            }

            return result.Value;
        }

        public static bool ForGraphQL(this IDomainResult result, IResolverContext ctx)
        {
            if (result.HasError)
            {
                var error = ErrorBuilder.New()
                  .SetCode(result.Code.ToString())
                  .SetMessage(result.Message)
                  .Build();
                ctx.ReportError(error);
                return false;
            }

            return true;
        }

        public static ActionResult ForRestApi<T>(this IDomainResult<T> result, Func<T, string> location = null) =>
            new ErrorModel<T>(result).BuildActionResult(location);

        public static ActionResult ForRestApi(this IDomainResult result) =>
            new ErrorModel(result).BuildActionResult();
    }
}
