using System;

namespace HoneyDo.Domain
{
	public class Todo
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }

		public Todo(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(nameof(name));
			}

			Id = Guid.NewGuid();
			Name = name;
		}
	}
}
