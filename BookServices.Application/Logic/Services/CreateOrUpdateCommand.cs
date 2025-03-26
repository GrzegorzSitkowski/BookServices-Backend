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

namespace BookServices.Application.Logic.Services
{
    public class CreateOrUpdateCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public int VenueId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public string Time { get; set; }
            public int CreatedBy { get; set; }
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

                Domain.Entities.Service? model = null;
                if (request.Id.HasValue)
                {
                    model = await _applicationDbContext.Services.FirstOrDefaultAsync(v => v.Id == request.Id && v.CreatedBy == account.Id);
                }
                else
                {
                    model = new Domain.Entities.Service()
                    {
                        CreatedBy = account.Id
                    };

                    _applicationDbContext.Services.Add(model);
                }

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.VenueId = request.VenueId;
                model.Name = request.Name;
                model.Description = request.Description;
                model.Price = request.Price;
                model.Time = request.Time;

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
                RuleFor(x => x.Price).NotEmpty().MaximumLength(20);
                RuleFor(x => x.Time).NotEmpty().MaximumLength(100);
            }
        }
    }
}
