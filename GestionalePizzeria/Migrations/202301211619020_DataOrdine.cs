namespace GestionalePizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataOrdine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ordini", "DataOrdine", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ordini", "DataOrdine");
        }
    }
}
