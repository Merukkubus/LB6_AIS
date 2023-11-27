namespace LB6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.BrandID);
            
            AddColumn("dbo.Notebooks", "BrandID", c => c.Int(nullable: false));
            CreateIndex("dbo.Notebooks", "BrandID");
            AddForeignKey("dbo.Notebooks", "BrandID", "dbo.Brands", "BrandID", cascadeDelete: true);
            DropColumn("dbo.Notebooks", "Brand");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notebooks", "Brand", c => c.String());
            DropForeignKey("dbo.Notebooks", "BrandID", "dbo.Brands");
            DropIndex("dbo.Notebooks", new[] { "BrandID" });
            DropColumn("dbo.Notebooks", "BrandID");
            DropTable("dbo.Brands");
        }
    }
}
