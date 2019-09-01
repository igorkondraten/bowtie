namespace BowTie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameSubArticle : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Articles", name: "SubArticleId", newName: "ParentArticleId");
            RenameIndex(table: "dbo.Articles", name: "IX_SubArticleId", newName: "IX_ParentArticleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Articles", name: "IX_ParentArticleId", newName: "IX_SubArticleId");
            RenameColumn(table: "dbo.Articles", name: "ParentArticleId", newName: "SubArticleId");
        }
    }
}
