using System;
using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for creating and updating todos.
    /// </summary>
    public class TodoCreateFormModel
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
    }
}
