using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookServices.Domain.Common;

namespace BookServices.Domain.Entities
{
    public class Reservations : DomainEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string WorkerName { get; set; }
        public string VenueName { get; set; }
        public int UserId { get; set; }
        public int VenueId { get; set; }
        public int WorkerId { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public Venue Venue { get; set; }
        public User User { get; set; }
        public Worker Worker { get; set; }
    }
}
