using BookServices.Application.Exceptions;
using BookServices.Application.Interfaces;
using BookServices.Application.Logic.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Application.Logic.Venues
{
    public class CreateOrUpdateCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string? PhoneNumber { get; set; }
            public string Street { get; set; }
            public string? PostCode { get; set; }
            public string City { get; set; }
        }

        public class Result
        {
            public required int Id { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext)
                : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                Domain.Entities.Venue? model = null;
                if (request.Id.HasValue)
                {
                    model = await _applicationDbContext.Venues.FirstOrDefaultAsync(v => v.Id == request.Id && v.CreatedBy == account.Id);
                }
                else
                {
                    model = new Domain.Entities.Venue()
                    {
                        CreatedBy = account.Id
                    };

                    _applicationDbContext.Venues.Add(model);
                }

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.Name = request.Name;
                model.Description = request.Description;
                model.PhoneNumber = request.PhoneNumber;
                model.Street = request.Street;
                model.PostCode = request.PostCode;
                model.City = request.City;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new Result()
                {
                    Id = model.Id
                };
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Description).MaximumLength(200);
                RuleFor(x => x.PhoneNumber).MaximumLength(20);
                RuleFor(x => x.Street).NotEmpty().MaximumLength(20);
                RuleFor(x => x.PostCode).MaximumLength(10);
                RuleFor(x => x.City).NotEmpty().MaximumLength(50);
            }
        }
    }
}
