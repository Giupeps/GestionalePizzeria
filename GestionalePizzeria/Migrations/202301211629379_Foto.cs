namespace GestionalePizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pizze", "Foto", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pizze", "Foto", c => c.String(nullable: false));
        }
    }
}
