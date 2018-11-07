using System;

namespace HoneyDo.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public string Picture { get; private set; }

        public Account(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Id = Guid.NewGuid();
            Name = name.Trim();
            IsEnabled = true;
            Picture = string.Empty;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name.Trim();
        }

        public void UpdatePicture(string picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                throw new ArgumentNullException(nameof(picture));

            Picture = picture.Trim();
        }
    }
}
