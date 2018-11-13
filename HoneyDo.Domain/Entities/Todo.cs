using System;

namespace HoneyDo.Domain.Entities
{
    public class Todo
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid OwnerId { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public DateTime? DueDate { get; private set; }

        [Obsolete("system constructor")]
        protected Todo() { }

        public Todo(string name, Account owner)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            Id = Guid.NewGuid();
            Name = name;
            OwnerId = owner.Id;
            CreateDate = DateTime.UtcNow;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public void Complete()
        {
            CompletedDate = DateTime.UtcNow;
        }

        public void UnComplete()
        {
            CompletedDate = null;
        }

        public void UpdateDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }
    }
}
