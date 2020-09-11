namespace Lab6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updates2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDTOes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        CustomerName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Date", c => c.String());
            DropColumn("dbo.Orders", "Number");
            DropTable("dbo.OrderDTOes");
        }
    }
}
