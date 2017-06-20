namespace ReportDeport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplatesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TemplateViewModels", "date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TemplateViewModels", "date");
        }
    }
}
