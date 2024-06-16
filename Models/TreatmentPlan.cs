using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;// foreign key is different library, suggested solution to [AllowNull] error

namespace HospitalAPI.Models
{
    public class TreatmentPlan {
        
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Plan_ID
        {
            get; set;
        }
        [ForeignKey("Patient")]
        public int Patient_ID
        {
            get; set;
        }
        [ForeignKey("Staff")]
        public int Staff_ID
        {
            get; set;
        }
        public int Plan_CycleLen
        {
            get; set;
        }
        public string Plan_Status
        {
            get; set;
        }

        public DateOnly Plan_Date
        {
            get; set;
        }
       
        [MaxLength(350)]
        [AllowNull]
        public string Plan_Observation
        {
            get; set;
        }

    }

}
