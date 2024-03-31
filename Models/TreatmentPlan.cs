using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // for date conversation
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;// foreign key is different library, suggested solution to [AllowNull] error

namespace HospitalAPI.Models
{
    public class TreatmentPlan {
        
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int TPlanEntry_ID  // this is autmatically generated, so we need Swagger wants to try to put value there, so will be a problem, so need a key for Swagger Schedme
        {
            get; set;
        }
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
        public int TPlan_CycleLen
        {
            get; set;
        }
        public string TPlan_Status
        {
            get; set;
        }

        public DateOnly Plan_Date
        {
            get; set;
        }
       
        [MaxLength(350)]
        [AllowNull]
        public string? TPlan_Observation
        {
            get; set;
        }
        public string TPlan_ActionType  // to be replaced with ENUM
        {
            get; set;
        }
        public int TPlan_ActionLink  // medication id or test resutlt id
        {
            get; set;
        }

    }

    public enum ActionType { prescription, test }
}
