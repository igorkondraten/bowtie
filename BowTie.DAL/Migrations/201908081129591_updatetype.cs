namespace BowTie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Content = c.String(),
                        SubArticleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.SubArticleId)
                .Index(t => t.SubArticleId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiagramUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JsonDiagram = c.String(),
                        UserId = c.Int(nullable: false),
                        Updates = c.String(),
                        Date = c.DateTime(nullable: false),
                        SavedDiagramId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SavedDiagrams", t => t.SavedDiagramId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SavedDiagramId);
            
            CreateTable(
                "dbo.SavedDiagrams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DiagramType = c.Byte(nullable: false),
                        ExpertCheck = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        EventName = c.String(),
                        PlaceId = c.Int(nullable: false),
                        EventTypeCode = c.Int(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeCode, cascadeDelete: true)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId)
                .Index(t => t.EventTypeCode);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                        ParentCode = c.Int(),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.EventTypes", t => t.ParentCode)
                .Index(t => t.ParentCode);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        DistrictId = c.Int(),
                        CityId = c.Int(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                        PasswordHash = c.String(),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.DiagramUpdates", "UserId", "dbo.Users");
            DropForeignKey("dbo.SavedDiagrams", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Places", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Places", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Places", "CityId", "dbo.Cities");
            DropForeignKey("dbo.EventTypes", "ParentCode", "dbo.EventTypes");
            DropForeignKey("dbo.Events", "EventTypeCode", "dbo.EventTypes");
            DropForeignKey("dbo.DiagramUpdates", "SavedDiagramId", "dbo.SavedDiagrams");
            DropForeignKey("dbo.Cities", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Districts", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Articles", "SubArticleId", "dbo.Articles");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Places", new[] { "CityId" });
            DropIndex("dbo.Places", new[] { "DistrictId" });
            DropIndex("dbo.Places", new[] { "RegionId" });
            DropIndex("dbo.EventTypes", new[] { "ParentCode" });
            DropIndex("dbo.Events", new[] { "EventTypeCode" });
            DropIndex("dbo.Events", new[] { "PlaceId" });
            DropIndex("dbo.SavedDiagrams", new[] { "EventId" });
            DropIndex("dbo.DiagramUpdates", new[] { "SavedDiagramId" });
            DropIndex("dbo.DiagramUpdates", new[] { "UserId" });
            DropIndex("dbo.Districts", new[] { "RegionId" });
            DropIndex("dbo.Cities", new[] { "DistrictId" });
            DropIndex("dbo.Articles", new[] { "SubArticleId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Places");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
            DropTable("dbo.SavedDiagrams");
            DropTable("dbo.DiagramUpdates");
            DropTable("dbo.Regions");
            DropTable("dbo.Districts");
            DropTable("dbo.Cities");
            DropTable("dbo.Articles");
        }
    }
}
