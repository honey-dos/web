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
        /// Name todo will be set to.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// DueDate todo will be set to.
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
