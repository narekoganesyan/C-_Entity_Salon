namespace SalonShop.ModelEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductSale")]
    public partial class ProductSale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime SaleDate { get; set; }

        public int ProductID { get; set; }

        public int Quanlity { get; set; }

        public int ClientServiceID { get; set; }

        public virtual Product Product { get; set; }
    }
}
