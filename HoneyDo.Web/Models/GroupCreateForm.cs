using System.ComponentModel.DataAnnotations;
using HoneyDo.Domain.Models;

namespace HoneyDo.Web.Models
{
    /// <summary> Structure for creating and updating groups.  </summary>
    public class GroupCreateForm : IGroupForm
    {
        /// <summary> The name the group will be set to.  </summary>
        [Required]
        public string Name { get; set; }
    }
}
