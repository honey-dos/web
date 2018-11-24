using HoneyDo.Domain.Entities;

namespace HoneyDo.Web.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public TokenModel(string token)
        {
            Token = token;
        }
    }
}
