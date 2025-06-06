﻿using BookServices.Application.Exceptions;
using BookServices.Application.Interfaces;
using BookServices.Application.Logic.Abstractions;
using BookServices.Domain.Entities;
using EFCoreSecondLevelCacheInterceptor;
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
    public static class CurrentAccountQuery
    {
        public class Request : IRequest<Result>
        {

        }

        public class Result
        {
            public required string Email { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            private readonly IAuthenticationDataProvider _authenticationDataProvider;

            public Handler(ICurrentAccountProvider currentAccountProvider, 
                IApplicationDbContext applicationDbContext,
                IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
            {
                _authenticationDataProvider = authenticationDataProvider;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = _authenticationDataProvider.GetUserId();

                if (userId.HasValue)
                {
                    var user = await _applicationDbContext.Users.Cacheable().FirstOrDefaultAsync(u => u.Id == userId.Value);
                    if (user != null)
                    {
                        return new Result()
                        {
                            Email = user.Email,
                        };
                    }
                }
                throw new UnauthorizedException();
            }
            
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
