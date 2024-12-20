using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.DAL.Models;

namespace Project.DAL.Data
{
    public class DBContextSeed
    {
        public static async Task SeedAsync(AppDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("SeedData");

            try
            {
                if (!context.Categories.Any())
                {
                    var categories = ReadCategoriesFromJsonFile("../Project.DAL/Data/DataSeed/categories.json");
                    context.Categories.AddRange(categories);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Categories seeded successfully.");
                }

               
                if (userManager.Users.All(u => u.UserName != "ahmedalielqazaz@gmail.com"))
                {
                    var admin = new ApplicationUser { FirstName="Ahmed", LastName="Elqazaz",
                        UserName = "ahmedalielqazaz", Email = "ahmedalielqazaz@gmail.com" };
                    await userManager.CreateAsync(admin, "#Ahmed_157");

                    // Assign role "Admin" if not already assigned
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Admin user created.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while seeding the database: {ex.Message}");
            }
        }



        private static List<Category> ReadCategoriesFromJsonFile(string filePath)
        {
            var categories = new List<Category>();

            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                categories = JsonConvert.DeserializeObject<List<Category>>(jsonData);
            }

            return categories;
        }
    }
}

