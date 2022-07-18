using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class SecurityPassword_User
    {
        [Key]
        public int UserPwdId { get; set; }
        public int PasswordId { get; set; }
        public int UserId { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> ScheduledExpirationDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
