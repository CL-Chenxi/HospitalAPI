using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HospitalAPI.Models
{
    public class TreatmentPlanEntry
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Entry_ID
        { get; set; }

        [ForeignKey("TreatmentPlan")]
        public int Plan_ID
        { get; set; }

        [ForeignKey("Staff")]
        public int Staff_ID
        { get; set; }

        public DateOnly Last_Update
        { get; set; }

        public string Entry_Type //says if a test, a scan, a prescription, etc
        {  get; set; }

        [AllowNull]
        public string Comment
        { get; set; }

        [ForeignKey("Drug")]
        [AllowNull]
        public int? Drug_ID //nullable when it's not a prescription
        {  get; set; }

        [AllowNull]
        public string? Posology //nullable when it's not a prescription
        { get; set; }

        [AllowNull]
        public string? UploadLink //updatable field for tests
        { get; set; }
    }


}
