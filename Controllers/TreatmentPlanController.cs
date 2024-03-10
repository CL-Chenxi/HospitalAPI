using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class TreatmentPlanController : Controller
    {
        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                              //_db is databse objects, through this object, can access PatientSet
                                                              //_db is a var name, we just named
                                                              //this is a property

        [HttpGet("GetTreatmentPlanById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TreatmentPlan))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult GetTreatmentPlanById(int patient_id, int plan_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            IEnumerable<TreatmentPlan> result = _db.TreatmentPlanSet //IEnummerable is a list, because, each plan contains multiple entries
                .Where(t => t.Patient_ID == patient_id && t.Plan_ID == plan_id) //entries must match patient id and plan id you are looking for
                .OrderByDescending(t => t.Plan_Date)
                .ToList();       //transform into  list
            
            return Results.Ok(result);
        }

        [HttpPost("CreateTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]

        public IResult Create([FromBody] TreatmentPlan treatmentplan)     //FromBody means body of request, contains my request creteria,
                                                              //create contains all info we need for create 
                                                              //map the body content over patient object
                                                              //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (treatmentplan == null)
            {
                return Results.BadRequest();
            }
            _db.TreatmentPlanSet.Add(treatmentplan);        //set is like a working progress
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdateTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Entry Updated")] //because u update a single line
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan Entry ID Not Found")]

        public IResult Update(int entry_id, [FromBody] TreatmentPlan plan)
        {
            var result = _db.TreatmentPlanSet.Find(entry_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                result.TPlan_Status = plan.TPlan_Status;
                result.TPlan_ActionLink = plan.TPlan_ActionLink;
                result.TPlan_Observation = plan.TPlan_Observation;
                result.TPlan_ActionType = plan.TPlan_ActionType;


                _db.TreatmentPlanSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();        //to say it did well
        }

        [HttpDelete("DeleteTreatmentPlanEntry")]        //delete entry only at this point?
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]

        public IResult Delete(int entry_id)
        {
            var result = _db.TreatmentPlanSet.Find(entry_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.TreatmentPlanSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
