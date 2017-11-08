namespace ART.Domotica.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasAlarmFiledAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DSFamilyTempSensor", "HasAlarm", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DSFamilyTempSensor", "HighAlarm", c => c.Decimal(precision: 6, scale: 3));
            AlterColumn("dbo.DSFamilyTempSensor", "LowAlarm", c => c.Decimal(precision: 6, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DSFamilyTempSensor", "LowAlarm", c => c.Decimal(nullable: false, precision: 6, scale: 3));
            AlterColumn("dbo.DSFamilyTempSensor", "HighAlarm", c => c.Decimal(nullable: false, precision: 6, scale: 3));
            DropColumn("dbo.DSFamilyTempSensor", "HasAlarm");
        }
    }
}