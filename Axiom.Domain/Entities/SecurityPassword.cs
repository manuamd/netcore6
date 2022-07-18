using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class SecurityPassword
    {
        [Key]
        public int PasswordId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHint { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> PwdExpireDate { get; set; }

    }
}
