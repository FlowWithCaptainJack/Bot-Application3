namespace Bot.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateCustomerMessageTimeStamp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomerMessages", "timestamp");
            AddColumn("dbo.CustomerMessages", "timestamp", c => c.Long(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CustomerMessages", "timestamp");
            AddColumn("dbo.CustomerMessages", "timestamp", c => c.DateTime(nullable: false));
        }
    }
}
