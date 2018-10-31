using System;

namespace HoneyDo.Web.Models
{
    public class TodoCreateFormModel
    {
        public string Name { get; set; }
        public bool IsValid => !string.IsNullOrEmpty(Name);
    }
}
