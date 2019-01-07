using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Infrastructure.Authentication
{
    /// <summary>
    /// Structure for 'logging in' a user.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Login provider (ie "Google")
        /// </summary>
        /// <value></value>
        [Required]
        public string Provider { get; set; }
        /// <summary>
        /// Provider's id for user.
        /// </summary>
        /// <value></value>
        [Required]
        public string ProviderId { get; set; }
        /// <summary>
        /// Provider's key for user, not required for current providers.
        /// </summary>
        /// <value></value>
        public string ProviderKey { get; set; }
    }
}
