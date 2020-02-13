using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Range(1, 10)]
        public int Priority { get; set; }
        public int ManagerId { get; set; }
        public int CustomerId { get; set; }
        public int PerformerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual Company Customer { get; set; }
        public virtual Company Performer { get; set; }
    }
}
