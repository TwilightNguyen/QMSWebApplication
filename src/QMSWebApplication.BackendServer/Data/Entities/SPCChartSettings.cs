using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("SPCChartSettings")]
    public class SPCChartSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ChartTypeId")]
        public int ChartTypeId { get; set; }

        [Column("PointQuantity")]
        public int PointQuantity { get; set; }

        [Column("FontSize")]
        public int FontSize { get; set; }

        [Column("YAxisTick")]
        public int YAxisTick { get; set; }

        [Column("XAxisTick")]
        public int XAxisTick { get; set; }
    }
}
