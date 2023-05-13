﻿namespace Khedmat_Shomar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        Browser = c.String(),
                        OS = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visits");
        }
    }
}
