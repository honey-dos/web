using System;
using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for creating and updating todos.
    /// </summary>
    public class TodoCreateForm
    {
        /// <summary>
        /// The name the todo will be set to.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The DueDate the todo will be set to (optional).
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// The group id of the group this todo will be assigned to (optional).
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}
