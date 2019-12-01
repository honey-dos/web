using HoneyDo.Domain.Models;

namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure for  updating todos.
    /// </summary>
    public class TodoUpdateForm : TodoCreateForm, ITodoUpdate
    {
        /// <summary>
        /// Sets or unsets completion date.
        /// </summary>
        public bool? IsComplete { get; set; }
        /// <summary>
        /// If true, will remove the todo from the group.
        /// </summary>
        public bool RemoveGroup { get; set; } = false;
        /// <summary>
        /// If true, will remove the due date from the todo.
        /// </summary>
        public bool RemoveDueDate { get; set; } = false;
        /// <summary>
        /// If true, will remove the assignee from the todo.
        /// </summary>
        public bool RemoveAssignee { get; set; } = false;
        /// <summary>
        /// The name the todo will be updated to, if set.
        /// </summary>
        public new string Name { get; set; }
    }
}
