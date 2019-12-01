using System;
using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HoneyDo.Web.Models
{
    /// <summary>Strucuture to communicate errors.</summary>
    public class ErrorModel<TEntity>
    {
        private DomainResultCode _code;
        private TEntity _value;

        /// <summary>The error.</summary>
        public string Error => _code.ToString();
        /// <summary>The error.</summary>
        public string Message { get; private set; }

        public ErrorModel(IDomainResult<TEntity> result)
        {
            _code = result.Code;
            Message = result.Message;
            _value = result.Value;
        }

        public ErrorModel(IDomainResult result)
        {
            _code = result.Code;
            Message = result.Message;
            _value = default(TEntity);
        }

        public ActionResult BuildActionResult(Func<TEntity, string> location = null)
        {
            switch (_code)
            {
                case DomainResultCode.Success:
                    return _value == null
                        ? new OkResult() as ActionResult
                        : new OkObjectResult(_value) as ActionResult;
                case DomainResultCode.Created:
                    var loc = location == null
                        ? string.Empty
                        : location.Invoke(_value);
                    return new CreatedResult(loc, _value);
                case DomainResultCode.Deleted:
                    return new NoContentResult();
                case DomainResultCode.InvalidArgument:
                case DomainResultCode.NotFound:
                    return new BadRequestObjectResult(this);
                case DomainResultCode.InsufficientPermissions:
                    return new ForbidResult();
                case DomainResultCode.Unauthorized:
                    return new UnauthorizedResult();
                default:
                    return new OkResult();
            }
        }
    }

    public class ErrorModel : ErrorModel<object>
    {
        public ErrorModel(IDomainResult result) : base(result) { }
    }

    /// <summary>Strucuture to communicate errors.</summary>
    public class ErrorModelB
    {
        /// <summary>The error.</summary>
        public string Error { get; set; }
        /// <summary>The error.</summary>
        public string Message { get; set; }
    }
}
