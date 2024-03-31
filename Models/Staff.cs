using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class Staff
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Staff_ID
        {
            get; set;
        }
        [MaxLength(50)]
        public string Staff_fName
        {
            get; set;
        }
        [MaxLength(50)]
        public string Staff_lName
        {
            get; set;
        }
        [MaxLength(50)]
        public int? Staff_PhoneNum
        {
            get; set;
        }
        public int Staff_Grade
        {
            get; set;
        }
    }

    public class Doctor : Staff
    {
        public Doctor()
        {
            Staff_Grade = 5;
        }

    }

    public class Technician : Staff 
    { 
        public Technician()
        {
            Staff_Grade = 3;
        }
    }
}
