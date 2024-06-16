using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HospitalAPI.Models
{
    public class Drug
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Drug_ID
        {
            get; set;
        }
        [MaxLength(50)]
        public string Drug_Name
        {
            get; set;
        }
        [MaxLength(50)]
        public string Drug_Dosage  // to research if a special 
        {
            get; set;
        }
        [AllowNull]
        public string Drug_AllergyList 
        { 
            get; set;
        }
        public Boolean Drug_Available
        {
            get; set;
        }

    }

}
