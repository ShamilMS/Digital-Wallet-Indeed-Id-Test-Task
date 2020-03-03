﻿using EWallet.Core.Models;
using EWallet.Core.Models.Domain;
using EWallet.Core.Models.DTO;
using EWallet.Core.Services.Application;
using EWallet.Core.Services.Persistence;
using EWallet.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWallet.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccoutController : Controller
    {
        private readonly IMediator mediator;

        public AccoutController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [Authorize(Policy = AuthorizationPolicies.AccountCreateOnly)]
        [HttpGet]
        [TypeFilter(typeof(ValidateCurrencyName))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<IActionResult> GetPermissionToken([FromQuery] AccountCreationRequest request)
        {
            var result = await mediator.Send(request);

            if (result.succeeded)
                return Ok();

            return BadRequest(new BadRequestResponse(result.errorMessage));
        }


    }
}