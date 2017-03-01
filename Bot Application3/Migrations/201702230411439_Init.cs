namespace Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ConversationId = c.String(),
                        Name = c.String(),
                        BotName = c.String(),
                        ServiceUrl = c.String(),
                        BotId = c.String(),
                        Customer_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Customers", t => t.Customer_UserId)
                .Index(t => t.Customer_UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        BotEnabled = c.Boolean(nullable: false),
                        ConversationId = c.String(),
                        Name = c.String(),
                        BotName = c.String(),
                        ServiceUrl = c.String(),
                        BotId = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.CustomerServers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ConversationId = c.String(),
                        Name = c.String(),
                        BotName = c.String(),
                        ServiceUrl = c.String(),
                        BotId = c.String(),
                        Customer_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Customers", t => t.Customer_UserId)
                .Index(t => t.Customer_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerServers", "Customer_UserId", "dbo.Customers");
            DropForeignKey("dbo.Admins", "Customer_UserId", "dbo.Customers");
            DropIndex("dbo.CustomerServers", new[] { "Customer_UserId" });
            DropIndex("dbo.Admins", new[] { "Customer_UserId" });
            DropTable("dbo.CustomerServers");
            DropTable("dbo.Customers");
            DropTable("dbo.Admins");
        }
    }
}
