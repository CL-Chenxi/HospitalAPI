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

        
        [HttpPost("CreateTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Medication Plan Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]

        public IResult Create([FromBody] MedicationPlan medicationplan)     //FromBody means body of request, contains my request creteria,
                                                                          //create contains all info we need for create 
                                                                          //map the body content over patient object
                                                                          //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (medicationplan == null)
            {
                return Results.BadRequest();
            }
            _db.MedicationPlanSet.Add(medicationplan);        //set is like a working progress
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
