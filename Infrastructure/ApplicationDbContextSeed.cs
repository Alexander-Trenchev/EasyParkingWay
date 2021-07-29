﻿using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ApplicationDbContextSeed
    {
 
        public static async Task Seed(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!context.Cities.Any())
            {
                //add rols
                var rolAdmin = new IdentityRole { Name = nameof(IdentityRoleEnum.Admin) };
                var rolUser = new IdentityRole { Name = nameof(IdentityRoleEnum.User) };
                await roleManager.CreateAsync(rolAdmin);
                await roleManager.CreateAsync(rolUser);

                //add users
                var admin = new IdentityUser { UserName = "admin@abv.bg", Email = "admin@abv.bg" };
                var result = await userManager.CreateAsync(admin, "123456");

                var user = new IdentityUser { UserName = "user@abv.bg", Email = "user@abv.bg" };
                await userManager.CreateAsync(user, "123456");

                await userManager.AddToRoleAsync(admin, rolAdmin.Name);
                await userManager.AddToRoleAsync(user, rolUser.Name);

                if (result.Succeeded)
                {


                    var sofia = new City()
                    {
                        Name = "Sofia",
                        Created = DateTime.Now
                    };

                    var plovdiv = new City()
                    {
                        Name = "Plovdiv",
                        Created = DateTime.Now,
                    };

                    var varna = new City()
                    {
                        Name = "Varna",
                        Created = DateTime.Now,
                    };

                    var burgas = new City()
                    {
                        Name = "Burgas",
                        Created = DateTime.Now
                    };

                    var ruse = new City()
                    {
                        Name = "Ruse",
                        Created = DateTime.Now
                    };

                    var blgrad = new City()
                    {
                        Name = "Blagoevgrad",
                        Created = DateTime.Now
                    };
                    await context.Cities.AddRangeAsync(sofia, plovdiv, varna, burgas, ruse, blgrad);
                    //await context.SaveChangesAsync(CancellationToken.None);

                    var parkingSofia_1 = new Parking()
                    {
                        Name = "Parking Sofia_1",
                        Created = DateTime.Now,
                        Address = "some street 29",
                        City = sofia,
                        PricePerParkingPlace = 10.00M
                    };

                    var parkingSofia_2 = new Parking()
                    {
                        Name = "Parking Sofia_2",
                        Created = DateTime.Now,
                        Address = "some street 28",
                        City = sofia,
                        PricePerParkingPlace = 15.00M
                    };

                    var parkingSofia_3 = new Parking()
                    {
                        Name = "Parking Sofia_3",
                        Created = DateTime.Now,
                        Address = "some street 30",
                        City = sofia,
                        PricePerParkingPlace = 20.00M
                    };

                    var parkingPlovdiv = new Parking()
                    {
                        Name = "Parking Plovdiv",
                        Created = DateTime.Now,
                        Address = "some street 29",
                        City = plovdiv,
                        PricePerParkingPlace = 8.00M
                    };

                    var parkingVarna = new Parking()
                    {
                        Name = "Parking Varna",
                        Created = DateTime.Now,
                        Address = "some street 29",
                        City = varna,
                        PricePerParkingPlace = 8.00M
                    };

                    await context.Parkings.AddRangeAsync(parkingSofia_1, parkingSofia_2, parkingSofia_3, parkingPlovdiv, parkingVarna);
                    await context.SaveChangesAsync(CancellationToken.None);
                    // ParkingPlace

                    for (int i = 1; i < 50; i++)
                    {

                        var parkingPlaceSofia_1 = new ParkingPlace()
                        {
                            Number = i,
                            Created = DateTime.Now,
                            Parking = parkingSofia_1,
                        };

                        var parkingPlaceSofia_2 = new ParkingPlace()
                        {
                            Number = i,
                            Created = DateTime.Now,
                            Parking = parkingSofia_2,
                        };

                        var parkingPlaceSofia_3 = new ParkingPlace()
                        {
                            Number = i,
                            Created = DateTime.Now,
                            Parking = parkingSofia_3,
                        };

                        var parkingPlacePlovdiv = new ParkingPlace()
                        {
                            Number = i,
                            Created = DateTime.Now,
                            Parking = parkingPlovdiv,
                        };

                        var parkingPlaceVarna = new ParkingPlace()
                        {
                            Number = i,
                            Created = DateTime.Now,
                            Parking = parkingVarna,
                        };

                        await context.ParkingPlaces.AddRangeAsync(parkingPlaceSofia_1, parkingPlaceSofia_2, parkingPlaceSofia_3, parkingPlacePlovdiv, parkingPlaceVarna);

                        if (i == 1 || i ==3 )
                        {

                            var payment_1 = new Payment()
                            {
                                RentFrom = DateTime.Now,
                                RentTo = DateTime.Now,
                                Created = DateTime.Now,
                                ParkingPlace = parkingPlaceSofia_1,
                                UserId = user.Id,
                                Amount = 100
                            };

                            var payment_2 = new Payment()
                            {
                                RentFrom = DateTime.Now,
                                RentTo = DateTime.Now,
                                Created = DateTime.Now,
                                ParkingPlace = parkingPlaceSofia_2,
                                UserId = user.Id,
                                Amount = 100
                            };
                            await context.Payments.AddRangeAsync(payment_1, payment_2);
                        }
                    }

                    await context.SaveChangesAsync(CancellationToken.None);
                }
            }
        }
    }
}
