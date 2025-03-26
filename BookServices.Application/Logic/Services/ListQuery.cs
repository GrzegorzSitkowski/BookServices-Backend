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

namespace BookServices.Application.Logic.Services
{
    public class ListQuery
    {
        public class  Request : IRequest<Result>
        {

        }

        public class Result
        {
            public List<Service> Services { get; set; } = new List<Service>();

            public class Service
            {
                public required int Id { get; set; }
                public string Name { get; set; }
                public string Price { get; set; }
                public string Time { get; set; }
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

                var data = await _applicationDbContext.Services.Where(v => v.CreatedBy == account.Id)
                    .Select(v => new Result.Service()
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Price = v.Price,
                        Time = v.Time
                    })
                    .ToListAsync();

                return new Result()
                {
                    Services = data
                };
            }
        }
    }
}
