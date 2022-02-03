namespace Messenger_Kings.Migrations
{
    using Messenger_Kings.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Messenger_Kings.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Messenger_Kings.Models.ApplicationDbContext";
        }

        protected override void Seed(Messenger_Kings.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);

                var driver = new ApplicationRole { Name = "Driver" };
                var role = new ApplicationRole { Name = "Admin" };
                var roles = new ApplicationRole { Name = "Client" };
                var em = new ApplicationRole { Name = "Employee" };

                manager.Create(role);
                manager.Create(roles);
                manager.Create(driver);
                manager.Create(em);

            }
            if (!context.Users.Any(u => u.UserName == "Admin@mk.co.za"))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "Admin@mk.co.za", Email = "Admin@mk.co.za" };
                var driverToInsert = new ApplicationUser { UserName = "Driver@mk.co.za", Email = "Driver@mk.co.za" };
                var driver1ToInsert = new ApplicationUser { UserName = "Driver1@mk.co.za", Email = "Driver1@mk.co.za" };
                var driver2ToInsert = new ApplicationUser { UserName = "Driver2@mk.co.za", Email = "Driver2@mk.co.za" };
                var driver3ToInsert = new ApplicationUser { UserName = "Driver3@mk.co.za", Email = "Driver3@mk.co.za" };
                var driver4ToInsert = new ApplicationUser { UserName = "Driver4@mk.co.za", Email = "Driver4@mk.co.za" };
                var employeeToInsert = new ApplicationUser { UserName = "Employee@mk.co.za", Email = "Employee@mk.co.za" };
               


                userManager.Create(userToInsert, "Qwerty1234*");
                userManager.Create(driverToInsert, "Qwerty1234*");
                userManager.Create(driver1ToInsert, "Qwerty1234*");
                userManager.Create(driver2ToInsert, "Qwerty1234*");
                userManager.Create(driver3ToInsert, "Qwerty1234*");
                userManager.Create(driver4ToInsert, "Qwerty1234*");
                userManager.Create(employeeToInsert, "Qwerty1234*");
                




                userManager.AddToRole(userToInsert.Id, "Admin");
                userManager.AddToRole(driverToInsert.Id, "Driver");
                userManager.AddToRole(driver1ToInsert.Id, "Driver");
                userManager.AddToRole(driver2ToInsert.Id, "Driver");
                userManager.AddToRole(driver3ToInsert.Id, "Driver");
                userManager.AddToRole(driver4ToInsert.Id, "Driver");
                userManager.AddToRole(employeeToInsert.Id, "Employee");

               

                context.Drivers.AddOrUpdate(x => x.Driver_ID,

                   new Driver()
                   {
                       Driver_ID = driverToInsert.Id,
                       Driver_IDNo = "9712215184080",
                       Driver_Name = "John",
                       Driver_Surname = "Doe",
                       Driver_Address = "10 Street, Durban",
                       Driver_Email = driverToInsert.Email,
                       Driver_CellNo = "0788556502",



                   },


                   new Driver()
                   {
                       Driver_ID = driver1ToInsert.Id,
                       Driver_IDNo = "9712215184081",
                       Driver_Name = "Bruno",
                       Driver_Surname = "Penandes",
                       Driver_Address = "50 Street, Durban",
                       Driver_Email = driver1ToInsert.Email,
                       Driver_CellNo = "0788556503",
                   },


                     new Driver()
                     {
                         Driver_ID = driver2ToInsert.Id,
                         Driver_IDNo = "9712215184082",
                         Driver_Name = "Haryy",
                         Driver_Surname = "Mag",
                         Driver_Address = "12 Street, Durban",
                         Driver_Email = driver2ToInsert.Email,
                         Driver_CellNo = "0788556504",
                     },


                     new Driver()
                     {
                         Driver_ID = driver3ToInsert.Id,
                         Driver_IDNo = "9712215184083",
                         Driver_Name = "James",
                         Driver_Surname = "Dean",
                         Driver_Address = "23 Street, Durban",
                         Driver_Email = driver3ToInsert.Email,
                         Driver_CellNo = "0788556505",
                     },


                     new Driver()
                     {
                         Driver_ID = driver4ToInsert.Id,
                         Driver_IDNo = "9712215184084",
                         Driver_Name = "Ak",
                         Driver_Surname = "Carry",
                         Driver_Address = "17 Street, Durban",
                         Driver_Email = driver4ToInsert.Email,
                         Driver_CellNo = "0788556506",
                     }



                    ) ;





            }
            if (!context.ClientCategories.Any(u => u.ClientCat_Type == "On Contract"))
            {
                context.ClientCategories.AddOrUpdate(x => x.ClientCat_ID,
                        new ClientCategory() { ClientCat_Type = "On Contract" },
                        new ClientCategory() { ClientCat_Type = "Pay as you go" }

                        );
            }

            if (!context.Rates.Any(u => u.Rate_ID == 1))
            {
                context.Rates.AddOrUpdate(x => x.Rate_ID,
                new Rate() { Rate_ID = 1, Base_Cost = 20, Rate_PerCM = 0.5M, Rate_PerKG = 5, Rate_PerKM = 0.5M, ClientCat_ID = 1 },
                new Rate() { Rate_ID = 1, Base_Cost = 30, Rate_PerCM = 0.5M, Rate_PerKG = 5, Rate_PerKM = 0.5M, ClientCat_ID = 2 }
            );




            }


















            if (!context.BankCategories.Any(r => r.Bank_Name == "African Bank"))
            {
                context.BankCategories.AddOrUpdate(
                new BankCategory()
                {
                    Bank_Name = "African Bank"
                },
                    new BankCategory()
                    {
                        Bank_Name = "Bidvest Bank"
                    },
                       new BankCategory()
                       {
                           Bank_Name = "Capitec Bank"
                       },
                          new BankCategory()
                          {
                              Bank_Name = "Nedbank"
                          },
                             new BankCategory()
                             {
                                 Bank_Name = "First National Bank"
                             },
                                new BankCategory()
                                {
                                    Bank_Name = "Absa"
                                }
                );


                if (!context.Status.Any(r => r.Storage_Status == "Shelf A"))
                {
                    context.Status.AddOrUpdate(
                    new Status()
                    {
                        Storage_Status = "Shelf A"
                    },
                      new Status()
                      {
                          Storage_Status = "Shelf B"
                      },
                      new Status()
                      {
                          Storage_Status = "Shelf C"
                      },
                      new Status()
                      {
                          Storage_Status = "Locker A"
                      },
                       new Status()
                       {
                           Storage_Status = "Locker B"
                       },
                       new Status()
                       {
                           Storage_Status = "Locker C"
                       },
                       new Status()
                       {
                           Storage_Status = "Out of Warehouse"
                       }






                    );

                }

                    context.AccountCategories.AddOrUpdate(
                    new AccountCategory()
                    {
                        Account_Type = "Saving"
                    },
                             new AccountCategory()
                             {
                                 Account_Type = "Cheque"
                             }
                    );

            }
        }
    }
}
