using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace otc_task.Models
{
    [Bind]
    public class Person
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Фамилия"), Required]
        public string LastName { get; set; }
        
        [Display(Name = "Имя"), Required]
        public string FirstName { get; set; }
        
        [Display(Name = "Отчество"), Required]
        public string MiddleName { get; set; }
        
        [Display(Name = "Возраст"), Required, Range(0, 200)]
        public int Age { get; set; }
    }
}