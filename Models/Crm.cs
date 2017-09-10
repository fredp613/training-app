using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Models
{
    public class Crm
    {
        [Key]
        public Guid CrmId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
