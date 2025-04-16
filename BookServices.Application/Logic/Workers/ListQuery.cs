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

namespace BookServices.Application.Logic.Workers
{
    public class ListQuery
    {
        public class  Request : IRequest<Result>
        {
            public int? VenueId { get; set; }
        }

        public class Result
        {
            public List<Worker> Workers { get; set; } = new List<Worker>();

            public class Worker
            {
                public required int Id { get; set; }
                public int VenueId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
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
                var venue = await _applicationDbContext.Venues.FirstOrDefaultAsync(v => v.Id == request.VenueId);

                var data = await _applicationDbContext.Workers.Where(d => d.CreatedBy == account.Id && d.VenueId == venue.Id)
                    .Select(d => new Result.Worker()
                    {
                        Id = d.Id,
                        VenueId = d.VenueId,
                        FirstName = d.FirstName,
                        LastName = d.LastName
                    })
                    .ToListAsync();

                return new Result()
                {
                    Workers = data
                };
            }
        }
    }
}
