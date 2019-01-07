using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Infrastructure.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Provider { get; set; }
        [Required]
        public string ProviderId { get; set; }
        public string ProviderKey { get; set; }
    }
}
