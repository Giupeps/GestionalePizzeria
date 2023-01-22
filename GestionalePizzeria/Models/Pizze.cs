namespace GestionalePizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pizze")]
    public partial class Pizze
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pizze()
        {
            Ordini = new HashSet<Ordini>();
        }

        [Key]
        public int IdPizza { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

       
        public string Foto { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Prezzo { get; set; }

        [Display(Name = "Pronta in")]
        public int TempoPreparazione { get; set; }

        [Required]
        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordini> Ordini { get; set; }
    }
}
