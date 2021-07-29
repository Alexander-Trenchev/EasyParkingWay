﻿using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Application.Payments.Queries.GetAllPaymentQuery;

namespace Web.Controllers
{
    [Authorize(Policy = nameof(IdentityPolicyEnum.WritePolicy))]
    public class AdminController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        private readonly ILogger<AdminController> _logger;
        private UserManager<IdentityUser> _userManager;

        public AdminController(ILogger<AdminController> logger, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> GetAllPayments(int? paymentId)
        {
            var result = await Mediator.Send(new GetAllPaymentsQuery { Id = paymentId});

            return View(result);
        }
    }
}
