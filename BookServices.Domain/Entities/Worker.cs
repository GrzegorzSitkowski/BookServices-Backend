using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookServices.Domain.Common;

namespace BookServices.Domain.Entities
{
    public class Worker : DomainEntity
    {
        public int VenueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreatedBy { get; set; }
        public Venue Venue { get; set; }
    }
}
