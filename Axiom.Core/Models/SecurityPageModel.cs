using Axiom.Application.Models;

namespace Axiom.Infrastructure.Models
{
    public class SecurityPageModel : BaseDtoModel
    {
		public int SpecificSecurityLookupId { get; set; }
		public int UserId { get; set; }
		public string SpecificSecurityName { get; set; }
		public bool UserSave { get; set; }
		public bool UserDelete { get; set; }
		public bool UserPrint { get; set; }
		public bool UserView { get; set; }

		public bool PageSave { get; set; }
		public bool PageDelete { get; set; }
		public bool PagePrint { get; set; }
		public bool PageView { get; set; }

		public string FormName { get; set; }
		public string FormTypeId { get; set; }
		public bool IsGlobalUser { get; set; }
		public string SecTypeDesc { get; set; }

		public string RoleId { get; set; }

		public SecurityPageModel() { }
		public SecurityPageModel(dynamic obj)
		{
			FillAllProperties(obj, this);
		}
	}
}
