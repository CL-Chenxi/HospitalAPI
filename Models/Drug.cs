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

        public string Drug_Name
        {
            get; set;
        }
        public string Drug_Dosage  // to research if a special 
        {
            get; set;
        }
        public string? Drug_AllergyList //parse string into list , https://stackoverflow.com/questions/5011467/convert-string-to-liststring-in-one-line
                                        //line 44-48 in TreatmentPlanContoller
                                        
        { 
            get; set;
        }

    }

}
