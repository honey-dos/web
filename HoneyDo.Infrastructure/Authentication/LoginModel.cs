using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Infrastructure.Authentication
{
    /// <summary>
    /// Structure for authenticating a user.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Login provider (ie "Google").
        /// </summary>
        [Required]
        public string Provider { get; set; }
        /// <summary>
        /// Provider's id for user.
        /// </summary>
        [Required]
        public string ProviderId { get; set; }
        /// <summary>
        /// Provider's key for user, not required for current providers.
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
