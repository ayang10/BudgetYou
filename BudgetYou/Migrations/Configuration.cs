namespace BudgetYou.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetYou.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        
        protected override void Seed(BudgetYou.Models.ApplicationDbContext context)
        {

            string[] categories =
           {
                "Automobile", "Bank charges", "Childcare", "Clothing", "Credit Card Fees", "Education",
                "Events", "Food", "Vacation", "Gifts", "Household", "Healthcare", "Insurance", "Job expenses",
                "Leisure (daily/non-vacation)", "Household", "Hobbies", "Loans", "Pet Care", "Savings", "Taxes", "Utilities"
            };

            if (context.Categories.Count() == 0)
            {
                foreach (var c in categories)
                {
                    context.Categories.Add(new Category { Name = c });
                }
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //lamda expression, check to see if context, roles, look for any roles in the table name is "Admin"
            //if(!roleManager.RoleExists("Admin")) this if functions does the same thing

            //check for demo household, add if not present

            if (!context.Households.Any(h => h.Name.Equals("Demo household")))
            {
                
            
            var household = context.Households.Add(new Household { Name = "Yang HouseHold" });

            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            if (!userManager.Users.Any(u => u.Email == "ayang014@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ayang014@gmail.com",
                    Email = "ayang014@gmail.com",
                    FirstName = "A",
                    LastName = "Yang",
                    HouseholdId = household.Id
                }, "Password-1");
            }

            }

        }
    }
}
