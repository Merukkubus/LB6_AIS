namespace LB6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notebooks",
                c => new
                    {
                        ID_notebook = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Brand = c.String(),
                        Resolution = c.String(maxLength: 50),
                        Frequency = c.String(maxLength: 50),
                        Weight = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID_notebook);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notebooks");
        }
    }
}
