using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class ConversionSecurityUserCustomer
    {
        [Key]
        public int UserCustomerId { get; set; }
        public int UserId { get; set; }
        public int CustomerKey { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }
}
