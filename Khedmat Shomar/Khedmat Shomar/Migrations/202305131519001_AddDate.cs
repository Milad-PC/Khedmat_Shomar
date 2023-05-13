namespace Khedmat_Shomar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visits", "VisitDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visits", "VisitDate");
        }
    }
}
