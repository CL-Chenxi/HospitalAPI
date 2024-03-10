using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class TestResult
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public string? TestRes_ID
        {
            get; set;
        }

        public DateOnly TestRes_Date
        {
            get; set;
        }
        public string TestRes_Type
        {
            get; set;
        }
        public string? TestRes_Observation
        {
            get; set;
        }
        public string TestRes_Link
        {
            get; set;
        }
        [ForeignKey("Staff")]
        public string Staff_ID
        {
            get; set;
        }


    }
}
