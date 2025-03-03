﻿using BookServices.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Domain.Entities
{
    public class User : DomainEntity
    {
        public required string FisrtName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public required string Type { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
    }
}
