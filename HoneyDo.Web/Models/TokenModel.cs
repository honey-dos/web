namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for returning JWT
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// JWT
        /// </summary>
        public string Token { get; set; }
        public TokenModel(string token)
        {
            Token = token;
        }
    }
}
