using HoneyDo.Domain.Entities;

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
        /// <value>JWT created by Honey-Dos</value>
        public string Token { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">JWT</param>
        public TokenModel(string token)
        {
            Token = token;
        }
    }
}
