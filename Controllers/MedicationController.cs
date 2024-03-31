using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class MedicationController : Controller
    {
        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                              //_db is databse objects, through this object, can access PatientSet
                                                              //_db is a var name, we just named
                                                              //this is a property

        [HttpGet("GetMedicationPlanById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MedicationPlan))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Medication Plan ID Not Found")]
        public IResult GetMedicationPlanById(int mplan_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            IEnumerable<MedicationPlan> result = _db.MedicationPlanSet //Lambda expressions and anonymous functions, to search through the table
                .Where(m => m.MedPlan_ID == mplan_id) //m represent each entry line, the shape it takes called lambda function
                .OrderByDescending(m => m.MedPlan_Date)     //lambda expression
                .ToList();       //transform into  list

            return Results.Ok(result);
        }

        
        [HttpPost("CreateMedicationPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Medication Plan Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]

        public IResult Create(int treatmentId, [FromBody] MedicationPlan medicationplan)     //FromBody means body of request, contains my request creteria,
                                                                          //create contains all info we need for create 
                                                                          //map the body content over patient object
                                                                          //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (medicationplan == null)
            {
                return Results.BadRequest();
            }
            TreatmentPlan tplan = _db.TreatmentPlanSet.Find(treatmentId);
            if (tplan == null) { return Results.BadRequest(); }

            //need validation
            Patient patient = _db.PatientSet.Find(tplan.Patient_ID);
            if (patient == null) { return Results.BadRequest(); }
            List<string> allergiesPatientList = patient.Patient_Allergy.Split(',').ToList();
            //list is called as above= above refers to Patient.cs patient allergy property
            if (allergiesPatientList.Count > 0 && tplan.TPlan_ActionType == ActionType.prescription.ToString())
            {
                Drug drug = _db.DrugSet.Find(medicationplan.Drug_ID);
                List<string> drugAllergiesList = drug.Drug_AllergyList.Split(',').ToList();
                if(allergiesPatientList.Intersect(drugAllergiesList).Count() > 0 ) 
                {
                    return Results.BadRequest(); //will need a custom message
                }

            }
            //if all good, update treatmentPlan with medicationId
            tplan.TPlan_ActionLink = medicationplan.MedPlan_ID;
            _db.MedicationPlanSet.Add(medicationplan);        //set is like a working progress
            _db.TreatmentPlanSet.Update(tplan);
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdateMedicationPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Medication Plan Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Medication Plan Entry ID Not Found")]

        public IResult Update(int entry_id, [FromBody] MedicationPlan plan)
        {
            var result = _db.MedicationPlanSet.Find(entry_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                result.MedPlan_Posology = plan.MedPlan_Posology;
              
                _db.MedicationPlanSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();        //to say it did well
        }

        [HttpDelete("DeleteMedicationPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Medication Plan Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Medication Plan ID Not Found")]

        public IResult Delete(int entry_id)
        {
            var result = _db.MedicationPlanSet.Find(entry_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.MedicationPlanSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
