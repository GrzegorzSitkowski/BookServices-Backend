using BookServices.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Domain.Entities
{
    public class Venue : DomainEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Street { get; set; }
        public string? PostCode { get; set; }
        public required string City { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
    }
}
