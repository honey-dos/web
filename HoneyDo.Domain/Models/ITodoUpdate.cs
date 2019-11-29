namespace HoneyDo.Domain.Models
{
    public interface ITodoUpdate : ITodoForm
    {
        bool? IsComplete { get; }
        bool RemoveGroup{ get; }
        bool RemoveDueDate { get; }
        bool RemoveAssignee { get; }
    }
}
