namespace GestionalePizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ordini",
                c => new
                    {
                        IdOrdine = c.Int(nullable: false, identity: true),
                        Quantità = c.Int(nullable: false),
                        IndirizzoSpedizione = c.String(),
                        Nota = c.String(),
                        OrdineConfermato = c.Boolean(nullable: false),
                        OrdineConsegnato = c.Boolean(nullable: false),
                        IdPizza = c.Int(nullable: false),
                        IdUtente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdOrdine)
                .ForeignKey("dbo.Pizze", t => t.IdPizza)
                .ForeignKey("dbo.Utenti", t => t.IdUtente)
                .Index(t => t.IdPizza)
                .Index(t => t.IdUtente);
            
            CreateTable(
                "dbo.Pizze",
                c => new
                    {
                        IdPizza = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Foto = c.String(nullable: false),
                        Prezzo = c.Decimal(nullable: false, storeType: "money"),
                        TempoPreparazione = c.Int(nullable: false),
                        Ingredienti = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdPizza);
            
            CreateTable(
                "dbo.Utenti",
                c => new
                    {
                        IdUtenti = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Nome = c.String(nullable: false),
                        Cognome = c.String(nullable: false),
                        Ruolo = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdUtenti);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ordini", "IdUtente", "dbo.Utenti");
            DropForeignKey("dbo.Ordini", "IdPizza", "dbo.Pizze");
            DropIndex("dbo.Ordini", new[] { "IdUtente" });
            DropIndex("dbo.Ordini", new[] { "IdPizza" });
            DropTable("dbo.Utenti");
            DropTable("dbo.Pizze");
            DropTable("dbo.Ordini");
        }
    }
}
