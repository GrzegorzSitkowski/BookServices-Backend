using BookServices.Application.Exceptions;
using BookServices.Application.Interfaces;
using BookServices.Application.Logic.Abstractions;
using BookServices.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookServices.Application.Logic.User
{
    public static class CreateUserWithAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public string Type { get; set; }
        }

        public class Result
        {
            public required int UserId { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IPasswordManager _passwordManager;

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IPasswordManager passwordManager) : base(currentAccountProvider, applicationDbContext)
            {
                _passwordManager = passwordManager;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = await _applicationDbContext.Users.AnyAsync(u => u.Email == request.Email);

                if (userExists)
                {
                    throw new ErrorException("AccountWithThisEmailAlreadyExists");
                }

                var utcNow = DateTime.Now;
                var user = new Domain.Entities.User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RegisterDate = utcNow,
                    Email = request.Email,
                    HashedPassword = "",
                    Type = request.Type,
                };

                user.HashedPassword = _passwordManager.HashPassword(request.Password);

                _applicationDbContext.Users.Add(user);

                var account = new Domain.Entities.Account()
                {
                    Name = request.Email,
                    CreateDate = utcNow,
                };

                _applicationDbContext.Accounts.Add(account);

                var accountUser = new AccountUser()
                {
                    Account = account,
                    User = user,
                };

                _applicationDbContext.AccountUsers.Add(accountUser);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new Result()
                {
                    UserId = user.Id,
                };
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Email).MaximumLength(100);

                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).MaximumLength(50);
            }
        }
    }
}
