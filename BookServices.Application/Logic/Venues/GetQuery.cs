using BookServices.Application.Exceptions;
using BookServices.Application.Interfaces;
using BookServices.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Application.Logic.Venues
{
    public class GetQuery
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string? PhoneNumber { get; set; }
            public string Street { get; set; }
            public string? PostCode { get; set; }
            public string City { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext)
                :base(currentAccountProvider, applicationDbContext)
            {

            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Venues.FirstOrDefaultAsync(v => v.Id == request.Id && v.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    Name = model.Name,
                    Description = model.Description,
                    PhoneNumber = model.PhoneNumber,
                    Street = model.Street,
                    PostCode = model.PostCode,
                    City = model.City
                };
            }
        }

    }
}
