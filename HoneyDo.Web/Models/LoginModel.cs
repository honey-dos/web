using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for authenticating a user
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Login provider (e.g. Google)
        /// </summary>
        [Required]
        public string Provider { get; set; }
        /// <summary>
        /// Provider's accessToken
        /// </summary>
        [Required]
        public string AccessToken { get; set; }
    }
}