namespace BowTie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Position");
        }
    }
}
