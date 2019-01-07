using System;
using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Models
{
    public class TodoCreateFormModel
    {
        [Required]
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
