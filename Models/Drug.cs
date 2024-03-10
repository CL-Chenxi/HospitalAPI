using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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

        public string Drug_Name
        {
            get; set;
        }
        public string Drug_Dosage  // to research if a special 
        {
            get; set;
        }
        public string? Drug_AllergyList // how to express list here? 
        { 
            get; set;
        }
    }
}
