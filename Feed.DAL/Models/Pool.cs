using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed.Domain.Models
{
    public class Pool
    {
        public int Id { get; set; }

        public string Title { get; set; }= null!;

        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosesAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int Status { get; set; }

        //Fks
        public string CreatedById { get; set; } = null!;
        
        //Navigation Properties
        public User CreatedBy { get; set; } = null!;

    }
}
