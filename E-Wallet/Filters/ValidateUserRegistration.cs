﻿using EWallet.Core.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Filters
{
    public sealed class ValidateUserRegistration : ValidationAttributeBase
    {
        private readonly IValidator<UserRegistrationRequest> userValidator;

        public ValidateUserRegistration(IValidator<UserRegistrationRequest> userValidator)
            => this.userValidator = userValidator;
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var registrationRequest = RetrieveArgument<UserRegistrationRequest>(context);

            var result = userValidator.Validate(registrationRequest);

            if (!result.IsValid)
            {
                ReturnBadRequest(context, error: string.Concat(result.Errors.Select(x => x.ErrorMessage)));
                return;
            }

            result = await userValidator.ValidateAsync(registrationRequest, ruleSet: "EmailIsNotRegistered");

            if (!result.IsValid)
            {
                ReturnBadRequest(context, error: string.Concat(result.Errors.Select(x => x.ErrorMessage)));
                return;
            }

            await next();
        }
    }
}
