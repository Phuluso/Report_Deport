namespace ReportDeport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactsAddedToDatabase1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        position = c.String(),
                        department = c.String(),
                        email = c.String(),
                        subject = c.String(),
                        message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactInfoes");
        }
    }
}
