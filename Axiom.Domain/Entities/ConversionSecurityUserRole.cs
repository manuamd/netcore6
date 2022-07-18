using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class ConversionSecurityUserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
