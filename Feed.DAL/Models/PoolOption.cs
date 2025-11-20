using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed.Domain.Models
{
    public class PoolOption
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string OptionText { get; set; } = null!;

        //in fuature we can add ImageUrl ,location ,delevery app link etc

        public DateTime CreatedAt { get; set; }

        //Fks
        public int PoolId { get; set; }
        
        //Navigation Properties
        public Pool Pool { get; set; } = null!;
        
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
