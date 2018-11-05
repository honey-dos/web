using HoneyDo.Domain.Entities;

namespace HoneyDo.Web.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string Error { get; set; }
        public TokenModel(string token, string error = "")
        {
            Token = token;
            Error = error;
        }
    }
}
