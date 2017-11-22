namespace ART.Domotica.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableDeviceBrokkerSetting3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceBrokerSetting", "Topic", c => c.String(nullable: false, maxLength: 32));
            CreateIndex("dbo.DeviceBrokerSetting", "Topic", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.DeviceBrokerSetting", new[] { "Topic" });
            DropColumn("dbo.DeviceBrokerSetting", "Topic");
        }
    }
}
