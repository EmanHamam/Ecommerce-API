using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                //create new users
                var user = new ApplicationUser
                {
                    FirstName = "Eman",
                    LastName = "Hamam",
                    Email = "emanhamam109@gmail.com",
                    UserName = "Eman_Hamam",
                    Address=
                    new Address
                    {
                        Street = "123 Main St",
                        City = "Qena",
                        State = "CA",
                        ZipCode = "12345",
                        Country = "Egypt"
                    }
                


                };
                await userManager.CreateAsync(user, "Em18@03");

                user = new ApplicationUser
                {
                    FirstName = "Harry",
                    LastName = "Potter",
                    Email = "harrypotter55@gmail.com",
                    UserName = "HarryPotter",
                    Address=
                    new Address
                    {
                        Street = "123 Main St",
                        City = "Cairo",
                        State = "CA",
                        ZipCode = "12345",
                        Country = "Egypt"
                    }
                


                };
                await userManager.CreateAsync(user, "P@ssword98@");

                user = new ApplicationUser
                {
                    FirstName = "Tom Marvolo",
                    LastName = "Riddle",
                    Email = "voldemort7@gmail.com",
                    UserName = "YouKnowWho",
                    Address = 
                    new Address
                        {
                            Street = "789 Oak St",
                            City = "Alexandria",
                            State = "CA",
                            ZipCode = "67890",
                            Country = "Egypt"
                        }
                    


                };
                await userManager.CreateAsync(user, "volde@12");

                user = new ApplicationUser
                {
                    FirstName = "Lol",
                    LastName = "Mohamed",
                    Email = "lol55@gmail.com",
                    UserName = "Lol",
                    Address=
                        new Address
                        {
                            Street = "456 Elm St",
                            City = "Giza",
                            State = "CA",
                            ZipCode = "54321",
                            Country = "Egypt"
                        }
                    


                };
                await userManager.CreateAsync(user, "$Lol98@");
            }
        }
    }
}
