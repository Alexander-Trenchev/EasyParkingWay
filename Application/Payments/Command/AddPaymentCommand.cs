﻿using Application.Common.Interfaces;
using Application.Payments.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Payments.Command
{
    public class AddPaymentCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public PaymentModel PaymentModel { get; set; }
    }

    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, bool>
    {
        private readonly IApplicationDbContext _dbContext;

        public AddPaymentCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var payments = await _dbContext.Payments
                .Where(x => x.ParkingPlaceId == request.PaymentModel.ParkingPlaceId)
                .ToListAsync();

            var isFree = payments.Any(x => IsFreeForPeriod(x, request.PaymentModel.RentFrom, request.PaymentModel.RentTo));

            if (!isFree)
            {
                return false;
            }

            var entity = new Payment();

            entity.Amount = request.PaymentModel.Amount;
            entity.ParkingPlaceId = request.PaymentModel.ParkingPlaceId;
            entity.RentFrom = request.PaymentModel.RentFrom;
            entity.RentTo = request.PaymentModel.RentTo;
            entity.UserId = request.UserId;
            try
            {
                await _dbContext.Payments.AddAsync(entity);
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private bool IsFreeForPeriod(Payment payment, DateTime requestFrom, DateTime requestTo)
        {
            return !(requestFrom.Date >= payment.RentFrom.Value.Date && requestFrom.Date <= payment.RentTo.Value.Date) || !(requestTo.Date >= payment.RentFrom.Value.Date && requestTo.Date <= payment.RentTo.Value.Date);
        }
    }
}
