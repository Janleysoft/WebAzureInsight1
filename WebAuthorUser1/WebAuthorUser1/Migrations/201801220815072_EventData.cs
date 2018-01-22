namespace WebAuthorUser1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InsightEventDatas", "MyTimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InsightEventDatas", "MyTimeStamp");
        }
    }
}
