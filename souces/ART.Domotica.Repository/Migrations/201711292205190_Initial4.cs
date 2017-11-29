namespace ART.Domotica.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial4 : DbMigration
    {
        #region Methods

        public override void Down()
        {
            DropForeignKey("dbo.SensorDatasheet", "SensorTypeId", "dbo.SensorType");
            DropIndex("dbo.SensorDatasheet", new[] { "SensorTypeId" });
            DropTable("dbo.SensorDatasheet");
        }

        public override void Up()
        {
            CreateTable(
                "dbo.SensorDatasheet",
                c => new
                    {
                        Id = c.Short(nullable: false),
                        SensorTypeId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.SensorTypeId })
                .ForeignKey("dbo.SensorType", t => t.SensorTypeId)
                .Index(t => t.SensorTypeId);
        }

        #endregion Methods
    }
}