//namespace Messenger_Kings.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;
    
//    public partial class InitialCreate : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "dbo.AccountCategories",
//                c => new
//                    {
//                        AccountCat_ID = c.Int(nullable: false, identity: true),
//                        Account_Type = c.String(nullable: false),
//                    })
//                .PrimaryKey(t => t.AccountCat_ID);
            
//            CreateTable(
//                "dbo.Banks",
//                c => new
//                    {
//                        Bank_ID = c.Int(nullable: false, identity: true),
//                        Bank_Account_Holder = c.String(nullable: false),
//                        Bank_Account_Number = c.String(nullable: false),
//                        Debit_Date = c.DateTime(nullable: false),
//                        Client_ID = c.String(maxLength: 128),
//                        BankCat_ID = c.Int(nullable: false),
//                        AccountCat_ID = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Bank_ID)
//                .ForeignKey("dbo.AccountCategories", t => t.AccountCat_ID, cascadeDelete: true)
//                .ForeignKey("dbo.BankCategories", t => t.BankCat_ID, cascadeDelete: true)
//                .ForeignKey("dbo.Clients", t => t.Client_ID)
//                .Index(t => t.Client_ID)
//                .Index(t => t.BankCat_ID)
//                .Index(t => t.AccountCat_ID);
            
//            CreateTable(
//                "dbo.BankCategories",
//                c => new
//                    {
//                        BankCat_ID = c.Int(nullable: false, identity: true),
//                        Bank_Name = c.String(nullable: false),
//                    })
//                .PrimaryKey(t => t.BankCat_ID);
            
//            CreateTable(
//                "dbo.Clients",
//                c => new
//                    {
//                        Client_ID = c.String(nullable: false, maxLength: 128),
//                        Client_IDNo = c.String(maxLength: 13),
//                        Client_Name = c.String(),
//                        Client_Surname = c.String(),
//                        Client_Cellnumber = c.String(),
//                        Client_Address = c.String(),
//                        Client_Email = c.String(),
//                        Client_Tellnum = c.String(),
//                        ClientCat_ID = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Client_ID)
//                .ForeignKey("dbo.ClientCategories", t => t.ClientCat_ID, cascadeDelete: true)
//                .Index(t => t.ClientCat_ID);
            
//            CreateTable(
//                "dbo.ClientCategories",
//                c => new
//                    {
//                        ClientCat_ID = c.Int(nullable: false, identity: true),
//                        ClientCat_Type = c.String(nullable: false),
//                    })
//                .PrimaryKey(t => t.ClientCat_ID);
            
//            CreateTable(
//                "dbo.Rates",
//                c => new
//                    {
//                        Rate_ID = c.Int(nullable: false, identity: true),
//                        Base_Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Rate_PerCM = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Rate_PerKG = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Rate_PerKM = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        ClientCat_ID = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Rate_ID)
//                .ForeignKey("dbo.ClientCategories", t => t.ClientCat_ID, cascadeDelete: true)
//                .Index(t => t.ClientCat_ID);
            
//            CreateTable(
//                "dbo.AspNetRoles",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        Name = c.String(nullable: false, maxLength: 256),
//                        Discriminator = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
//            CreateTable(
//                "dbo.AspNetUserRoles",
//                c => new
//                    {
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        RoleId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.UserId, t.RoleId })
//                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId)
//                .Index(t => t.RoleId);
            
//            CreateTable(
//                "dbo.Bookings",
//                c => new
//                    {
//                        Book_ID = c.Int(nullable: false, identity: true),
//                        Book_PickupDate = c.DateTime(nullable: false),
//                        Book_DeliveryDate = c.DateTime(nullable: false),
//                        Book_RecipientName = c.String(nullable: false),
//                        Book_RecipientSurname = c.String(nullable: false),
//                        Book_RecipientNumber = c.String(nullable: false),
//                        Book_DeliveryNote = c.String(maxLength: 250),
//                        Book_TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        BookStatus = c.Boolean(nullable: false),
//                        BookDecline = c.Boolean(nullable: false),
//                        paymentstatus = c.Boolean(nullable: false),
//                        Quote_ID = c.Int(nullable: false),
//                        Client_ID = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Book_ID)
//                .ForeignKey("dbo.Clients", t => t.Client_ID)
//                .ForeignKey("dbo.Quotes", t => t.Quote_ID, cascadeDelete: true)
//                .Index(t => t.Quote_ID)
//                .Index(t => t.Client_ID);
            
//            CreateTable(
//                "dbo.Orders",
//                c => new
//                    {
//                        Order_ID = c.Int(nullable: false, identity: true),
//                        Order_DateTime = c.DateTime(nullable: false),
//                        Book_ID = c.Int(nullable: false),
//                        Driver_ID = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Order_ID)
//                .ForeignKey("dbo.Bookings", t => t.Book_ID, cascadeDelete: true)
//                .ForeignKey("dbo.Drivers", t => t.Driver_ID)
//                .Index(t => t.Book_ID)
//                .Index(t => t.Driver_ID);
            
//            CreateTable(
//                "dbo.Drivers",
//                c => new
//                    {
//                        Driver_ID = c.String(nullable: false, maxLength: 128),
//                        Driver_IDNo = c.String(),
//                        Driver_Name = c.String(),
//                        Diver_Image = c.Binary(),
//                        Driver_Surname = c.String(),
//                        Driver_CellNo = c.String(),
//                        Driver_Address = c.String(),
//                        Driver_Email = c.String(),
//                    })
//                .PrimaryKey(t => t.Driver_ID);
            
//            CreateTable(
//                "dbo.Trackings",
//                c => new
//                    {
//                        Track_ID = c.String(nullable: false, maxLength: 128),
//                        Track_Message = c.String(),
//                        Order_Status = c.String(),
//                        Out_for_delivery = c.String(),
//                        Pickup_Status = c.String(),
//                        Delivery_Status = c.String(),
//                        Order_ID = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Track_ID)
//                .ForeignKey("dbo.Orders", t => t.Order_ID, cascadeDelete: true)
//                .Index(t => t.Order_ID);
            
//            CreateTable(
//                "dbo.Quotes",
//                c => new
//                    {
//                        Quote_ID = c.Int(nullable: false, identity: true),
//                        Quote_Date = c.DateTime(nullable: false),
//                        Quote_PickupAddress = c.String(nullable: false),
//                        Quote_DeliveryAddress = c.String(nullable: false),
//                        Quote_Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Quote_Description = c.String(nullable: false),
//                        Quote_Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Item_Quantity = c.Int(nullable: false),
//                        Quote_length = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Quote_Height = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Quote_Width = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        Quote_Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        ClientCat_ID = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Quote_ID)
//                .ForeignKey("dbo.ClientCategories", t => t.ClientCat_ID, cascadeDelete: true)
//                .Index(t => t.ClientCat_ID);
            
//            CreateTable(
//                "dbo.Documents",
//                c => new
//                    {
//                        Documents_ID = c.Int(nullable: false, identity: true),
//                        Document_ID = c.Binary(),
//                        Document_Residence = c.Binary(),
//                        Document_Statement = c.Binary(),
//                        Client_ID = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Documents_ID)
//                .ForeignKey("dbo.Clients", t => t.Client_ID)
//                .Index(t => t.Client_ID);
            
//            CreateTable(
//                "dbo.AspNetUsers",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        Email = c.String(maxLength: 256),
//                        EmailConfirmed = c.Boolean(nullable: false),
//                        PasswordHash = c.String(),
//                        SecurityStamp = c.String(),
//                        PhoneNumber = c.String(),
//                        PhoneNumberConfirmed = c.Boolean(nullable: false),
//                        TwoFactorEnabled = c.Boolean(nullable: false),
//                        LockoutEndDateUtc = c.DateTime(),
//                        LockoutEnabled = c.Boolean(nullable: false),
//                        AccessFailedCount = c.Int(nullable: false),
//                        UserName = c.String(nullable: false, maxLength: 256),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
//            CreateTable(
//                "dbo.AspNetUserClaims",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        ClaimType = c.String(),
//                        ClaimValue = c.String(),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
            
//            CreateTable(
//                "dbo.AspNetUserLogins",
//                c => new
//                    {
//                        LoginProvider = c.String(nullable: false, maxLength: 128),
//                        ProviderKey = c.String(nullable: false, maxLength: 128),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
            
//        }
        
//        public override void Down()
//        {
//            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
//            DropForeignKey("dbo.Documents", "Client_ID", "dbo.Clients");
//            DropForeignKey("dbo.Bookings", "Quote_ID", "dbo.Quotes");
//            DropForeignKey("dbo.Quotes", "ClientCat_ID", "dbo.ClientCategories");
//            DropForeignKey("dbo.Trackings", "Order_ID", "dbo.Orders");
//            DropForeignKey("dbo.Orders", "Driver_ID", "dbo.Drivers");
//            DropForeignKey("dbo.Orders", "Book_ID", "dbo.Bookings");
//            DropForeignKey("dbo.Bookings", "Client_ID", "dbo.Clients");
//            DropForeignKey("dbo.Banks", "Client_ID", "dbo.Clients");
//            DropForeignKey("dbo.Rates", "ClientCat_ID", "dbo.ClientCategories");
//            DropForeignKey("dbo.Clients", "ClientCat_ID", "dbo.ClientCategories");
//            DropForeignKey("dbo.Banks", "BankCat_ID", "dbo.BankCategories");
//            DropForeignKey("dbo.Banks", "AccountCat_ID", "dbo.AccountCategories");
//            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
//            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
//            DropIndex("dbo.AspNetUsers", "UserNameIndex");
//            DropIndex("dbo.Documents", new[] { "Client_ID" });
//            DropIndex("dbo.Quotes", new[] { "ClientCat_ID" });
//            DropIndex("dbo.Trackings", new[] { "Order_ID" });
//            DropIndex("dbo.Orders", new[] { "Driver_ID" });
//            DropIndex("dbo.Orders", new[] { "Book_ID" });
//            DropIndex("dbo.Bookings", new[] { "Client_ID" });
//            DropIndex("dbo.Bookings", new[] { "Quote_ID" });
//            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
//            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
//            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
//            DropIndex("dbo.Rates", new[] { "ClientCat_ID" });
//            DropIndex("dbo.Clients", new[] { "ClientCat_ID" });
//            DropIndex("dbo.Banks", new[] { "AccountCat_ID" });
//            DropIndex("dbo.Banks", new[] { "BankCat_ID" });
//            DropIndex("dbo.Banks", new[] { "Client_ID" });
//            DropTable("dbo.AspNetUserLogins");
//            DropTable("dbo.AspNetUserClaims");
//            DropTable("dbo.AspNetUsers");
//            DropTable("dbo.Documents");
//            DropTable("dbo.Quotes");
//            DropTable("dbo.Trackings");
//            DropTable("dbo.Drivers");
//            DropTable("dbo.Orders");
//            DropTable("dbo.Bookings");
//            DropTable("dbo.AspNetUserRoles");
//            DropTable("dbo.AspNetRoles");
//            DropTable("dbo.Rates");
//            DropTable("dbo.ClientCategories");
//            DropTable("dbo.Clients");
//            DropTable("dbo.BankCategories");
//            DropTable("dbo.Banks");
//            DropTable("dbo.AccountCategories");
//        }
//    }
//}
