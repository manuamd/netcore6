using Axiom.Application.Models;
using System.Collections.Generic;

namespace Axiom.Infrastructure.Models
{
    public class ClientHeaderInfo : BaseDtoModel
    {
        public string Initial { get; set; }
        public string DOB { get; set; }
        public string Provider { get; set; }
        public string CISId { get; set; }
        public string SSN { get; set; }
        public string AHCCCSId { get; set; }
        public string BHMISId { get; set; }
        public string MedicareId { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string workPhone { get; set; }

        public string Population { get; set; }
        public string Title { get; set; }
        public string EnrollmentDate { get; set; }
        public string ClosureDate { get; set; }
        public int Pregnant { get; set; }
        public int EnrollmentId { get; set; }
        public int? ProgramIndicatorId { get; set; }
        public string ProgramIndicator { get; set; }

        public string programAbbr { get; set; }
        public int? rbhaindicator { get; set; }
        public string LanguageDesc { get; set; }
        public string name { get { return (Initial != null) ? FirstName + " " + Initial + " " + LastName : FirstName + " " + LastName; } }
        public int Age { get; set; }
        public string Photo { get; set; }
        public int ProviderId { get; set; }

        public string Site { get; set; }
        public string Payor { get; set; }
        public string FundingCategory { get; set; }
        public string FundingCategoryCode { get; set; }
        public bool HidePatientPanel { get; set; }

        public string COECOT { get; set; }
        public string NoteType { get; set; }
        public string NoteTypeText { get; set; }
        public string ClientAddress { get; set; }
        public string RateCodeDescription { get; set; }
        public string RateCodeDesc { get; set; }
        public string PronounId { get; set; }
        public string PreferredPronoun { get; set; }
        public string OtherFirstName { get; set; }
        public string OtherLastName { get; set; }

        public List<string> Schools { get; set; }
        public string PortalLogin { get; set; }
        public string LastPortalLogin { get; set; }

        public string ClientCity { get; set; }
        public string ClientState { get; set; }
        public string ClientZipCode { get; set; }

        public ClientHeaderInfo()
        {

        }

        public ClientHeaderInfo(dynamic obj)
        {
            FillAllProperties(obj, this);
        }
    }
}
