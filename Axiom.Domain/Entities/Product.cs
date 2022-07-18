using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Axiom.Domain.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }

        public string Description { get; set; }
        public decimal Rate { get; set; }
        public int BrandId { get; set; }
    }
}
