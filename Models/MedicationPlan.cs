using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class MedicationPlan
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int MPlanEntry_ID
        {
            get; set;
        }

        public int MedPlan_ID
        {
            get; set;
        }

        public DateOnly MedPlan_Date
        {
            get; set;
        }
        public string? MedPlan_Posology
        {
            get; set;
        }
        [ForeignKey("Drug")]
        public int Drug_ID
        {
            get; set;
        }
    }
}
