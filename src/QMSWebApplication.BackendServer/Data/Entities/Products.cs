using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Product")]
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column("AreaId")]
        public int? AreaId { get; set; }


        [Column("InspPlanId")]
        public int? InspPlanId { get; set; }


        [Column("Enabled")]
        public bool Enabled { get; set; } = false;

        [MaxLength(100)]
        [Column("ModelInternal", TypeName = "nvarchar(100)")]
        public string? ModelInternal { get; set; }

        [MaxLength(100)]
        [Column("CustomerName", TypeName = "nvarchar(100)")]
        public string? CustomerName { get; set; }

        [MaxLength(100)]
        [Column("Notes", TypeName = "nvarchar(100)")]
        public string? Notes { get; set; }

        [Column("VialFixture")]
        public int? VialFixture { get; set; }

        [MaxLength(100)]
        [Column("Description", TypeName = "nvarchar(100)")]
        public string? Description { get; set; }

        [Column("MoldQuantity")]
        public int? MoldQuantity { get; set; }

        [Column("CavityQuantity")]
        public int? CavityQuantity { get; set; }
    }
}
