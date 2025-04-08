using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookServices.Domain.Common;

namespace BookServices.Domain.Entities
{
    public class Service : DomainEntity
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Time { get; set; }
        public int CreatedBy { get; set; }
        public Venue Venue { get; set; }
    }
}
