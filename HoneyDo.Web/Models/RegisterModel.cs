using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for registering a new account.
    /// </summary>
    public class RegisterModel : LoginModel
    {
        /// <summary>
        /// Name the new account will be set to.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// UserName the new account will be set to, must app unique.
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// Picture the new account will have (optional).
        /// </summary>
        public string Picture { get; set; }
    }
}