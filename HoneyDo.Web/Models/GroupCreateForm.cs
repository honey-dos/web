using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for creating and updating todos.
    /// </summary>
    public class GroupCreateForm
    {
        /// <summary>
        /// The name the todo will be set to.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
