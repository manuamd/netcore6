using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Axiom.Domain.Entities
{
    public partial class SecurityUser
    {
        [Key]
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string OTPEmail { get; set; }
        public string CalendarGUID { get; set; }
        public Nullable<int> CalendarDaysBack { get; set; }
        public Nullable<int> CalendarDaysForward { get; set; }
    }
}
