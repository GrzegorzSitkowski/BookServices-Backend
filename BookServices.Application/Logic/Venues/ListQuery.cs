using BookServices.Application.Interfaces;
using BookServices.Application.Logic.Abstractions;
using BookServices.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Application.Logic.Venues
{
    public class ListQuery
    {
        public class  Request : IRequest<Result>
        {

        }

        public class Result
        {
            public List<Venue> Venues { get; set; } = new List<Venue>();

            public class Venue
            {
                public required int Id { get; set; }
                public string Name { get; set; }
                public string PhoneNumber { get; set; }
                public string Street { get; set; }
                public string City { get; set; }
            }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) 
                : base(currentAccountProvider, applicationDbContext)
            {
                
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var data = await _applicationDbContext.Venues.Where(v => v.CreatedBy == account.Id)
                    .OrderByDescending(v => v.CreateDate)
                    .Select(v => new Result.Venue()
                    {
                        Id = v.Id,
                        Name = v.Name,
                        PhoneNumber = v.PhoneNumber,
                        Street = v.Street,
                        City = v.City
                    })
                    .ToListAsync();

                return new Result()
                {
                    Venues = data
                };
            }
        }
    }
}
