using System;
using System.ComponentModel.DataAnnotations;
using HoneyDo.Domain.Models;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for creating  todos.
    /// </summary>
    public class TodoCreateForm : ITodoForm
    {
        /// <summary>
        /// The name the todo will be set to.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The DueDate the todo will be set to.
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// The group id of the group this todo will be assigned to.
        /// </summary>
        public Guid? GroupId { get; set; }
        /// <summary>
        /// The account id of the user to assign this todo too.
        /// </summary>
        public Guid? AssigneeId { get; set; }
    }
}
