namespace GestionalePizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Utenti", "Ruolo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Utenti", "Ruolo", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
