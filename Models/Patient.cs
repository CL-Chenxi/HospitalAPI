using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;// foreign key is different library, suggested solution to [AllowNull] error

namespace HospitalAPI.Models
{
    public class Patient
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]  //because database will generate patient ID for me, so we need to enable Swagger to not inject/insert a value
        public int Patient_ID 
        {
            get; set;
        }
        [MaxLength(50)]
        [AllowNull]  // for databse when it saving to Database
        public string? Patient_fName   // without ?, can still be Null, used more for return values in a function, with?, ti will return either s tring, or Null
        {
            get; set;
        }
        public string? Patient_lName
        {
            get; set;
        }
        [MaxLength(50)]
        public DateOnly Patient_DoB 
        { 
            get; set; 
        }
        public int Patient_PhoneNum 
        { 
            get; set;
        }
        public string Patient_Allergy 
        { 
            get; set;
        }
        
    }
}
