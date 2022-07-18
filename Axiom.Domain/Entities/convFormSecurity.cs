using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class convFormSecurity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }
}
