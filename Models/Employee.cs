using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я""'\s-]*$")]
        public string Surname { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я""'\s-]*$")]
        public string Name { get; set; }

        [StringLength(15)]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я""'\s-]*$")]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
