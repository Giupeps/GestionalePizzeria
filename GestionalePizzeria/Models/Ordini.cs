namespace GestionalePizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [Key]
        public int IdOrdine { get; set; }

        public int Quantità { get; set; }

        public string IndirizzoSpedizione { get; set; }

        public DateTime DataOrdine { get; set; }

        public string Nota { get; set; }

        public bool OrdineConfermato { get; set; }

        public bool OrdineConsegnato { get; set; }

        public int IdPizza { get; set; }

        public int IdUtente { get; set; }

        public virtual Pizze Pizze { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
