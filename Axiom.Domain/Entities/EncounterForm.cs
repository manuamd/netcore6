using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Axiom.Domain.Entities
{
    [Table("EncounterForm")]
    public partial class EncounterForm
    {
        public int EncounterFormId { get; set; }
        public string FormName { get; set; }
        public string TemplateFile { get; set; }
        public bool SitesByClient { get; set; }
        public bool SitesByEmployee { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<bool> FormBeingUsed { get; set; }
        public Nullable<bool> ShowServiceCodeNeeds { get; set; }
        public Nullable<bool> ShowEncounterServiceNote { get; set; }
        public Nullable<bool> ShowNeed { get; set; }
        public Nullable<bool> ShowObjective { get; set; }
        public Nullable<bool> ShowAllergy { get; set; }
        public Nullable<bool> ShowPrescriptionItem { get; set; }
        public Nullable<bool> ShowPCPMed { get; set; }
        public Nullable<bool> ShowPrescriptionDiscontinuation { get; set; }
        public Nullable<bool> ShowAIMS { get; set; }
        public Nullable<bool> ShowVitals { get; set; }
        public Nullable<bool> ShowInjection { get; set; }
        public Nullable<bool> ShowLabOrder { get; set; }
        public Nullable<bool> ShowLicense { get; set; }
        public Nullable<bool> ShowQualification { get; set; }
        public Nullable<bool> ShowEducation { get; set; }
        public Nullable<bool> ShowProblemList { get; set; }
        public Nullable<bool> ShowServicePlan { get; set; }
        public Nullable<bool> ShowRelDX { get; set; }
        public Nullable<bool> ShowICD10Codes { get; set; }
        public Nullable<int> ServiceText { get; set; }
        public Nullable<int> ServiceIdentifiedNeedsLimiter { get; set; }
        public Nullable<int> Part2Data { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<bool> LoadPreviousNote { get; set; }
        public string ROSColorPositive { get; set; }
        public string ROSColorNegative { get; set; }
        public Nullable<bool> Interpreter { get; set; }
        public Nullable<bool> Intervention { get; set; }
        public Nullable<bool> EncounterType { get; set; }
        public Nullable<bool> SelectProvider { get; set; }
        public Nullable<bool> ShowCardView { get; set; }
    }
}
