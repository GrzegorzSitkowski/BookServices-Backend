using BookServices.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Domain.Entities
{
    public class User : DomainEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Type { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
    }
}
