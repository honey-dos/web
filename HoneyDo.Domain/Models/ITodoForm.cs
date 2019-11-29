using System;

namespace HoneyDo.Domain.Models
{
    public interface ITodoForm
    {
        string Name { get; }
        DateTime? DueDate { get; }
        Guid? GroupId { get; }
        Guid? AssigneeId { get; }
    }
}
