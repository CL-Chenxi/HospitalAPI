using HospitalAPI.Models;
using HospitalAPI.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class TreatmentPlanController : Controller
    {
        private TreatmentService _treatmentService = new TreatmentService();
        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                              //_db is databse objects, through this object, can access PatientSet
                                                              //_db is a var name, we just named
                                                              //this is a property

        [HttpGet("GetTreatmentPlanById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TreatmentPlanDto))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult GetTreatmentPlanById(int plan_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            var result = _treatmentService.GetTreatmentPlanById(plan_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        [HttpGet("GetAllTreatmentPlansForPatient")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<TreatmentPlanDto>))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient Not Found")]
        public IResult GetAllTreatmentPlansForPatient(int patient_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            try
            {
                var result = _treatmentService.GetAllTreatmentPlansForPatient(patient_id);
                return Results.Ok(result);
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }

        }

        [HttpPost("CreateNewTreatmentPlan")]
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Created", Type = typeof(TreatmentPlanDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]

        public IResult CreateTreatmentPlan([FromBody] NewPlanRequest newTreatmentPlan)     //FromBody means body of request, contains my request criteria,
                                                                              //create contains all info we need for create 

        {
            if (newTreatmentPlan == null)
            {
                return Results.BadRequest();
            }
            try
            {
                TreatmentPlanDto newCreatedPlan = _treatmentService.CreateNewTreatmentPlan(newTreatmentPlan);
                return Results.Ok(newCreatedPlan); //return the ID of the newly created plan
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }

        }

        [HttpPut("UpdateTreatmentPlan")]
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Updated")] //because u update a single line
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]

        public IResult UpdateTreatmentPlan([FromBody] UpdatePlanRequest planRequest)
        {
            if (planRequest == null)
            {
                return Results.BadRequest();
            }
            try
            {
                _treatmentService.UpdateTreatmentPlan(planRequest);
                return Results.Ok();        //to say it did well
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        }

        [HttpDelete("DeleteTreatmentPlan")]
        [SwaggerResponse(StatusCodes.Status200OK, "Treatment Plan Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]

        public IResult DeleteTreatmentPlan(int plan_id)
        {
            try
            {
                _treatmentService.DeleteTreatmentPlan(plan_id);
                return Results.Ok();        //to say it did well
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        }


        [HttpGet("GetAllEntriesForPlan")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<TreatmentEntryDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult GetAllEntriesForPlan(int plan_id)
        {
            try
            {
                var resultList = _treatmentService.GetAllPlanEntriesForPlanId(plan_id);
                return Results.Ok(resultList);
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        }

        [HttpPut("UpdateTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plan Entry Updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult UpdateTreatmentPlanEntry([FromBody] UpdateEntryRequest plan_entry)
        {
            if (plan_entry == null) return Results.BadRequest("Null body");
            try
            {
                _treatmentService.UpdatePlanEntry(plan_entry);
                return Results.Ok();
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }

        }

        [HttpPost("AddTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plan Entry added", Type = typeof(TreatmentEntryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult AddTreatmentPlanEntry([FromBody] NewEntryRequest plan_entry)
        {
            try
            {
                TreatmentEntryDto newEntry =  _treatmentService.CreateNewEntry(plan_entry);
                return Results.Ok(newEntry);
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        }

        [HttpGet("GetTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TreatmentEntryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Treatment Plan ID Not Found")]
        public IResult GetTreatmentPlanEntry(int entry_id)
        {
            var result = _treatmentService.GetTreatmentPlanEntryById(entry_id);
            if (result == null) return Results.NotFound("Treatment Plan ID #"+entry_id+" Not Found");
            else return Results.Ok(result);
        }

        [HttpDelete("DeleteTreatmentPlanEntry")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plan Entry Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plan Entry ID Not Found")]
        public IResult DeleteTreatmentPlanEntry(int entry_id)
        {
            try
            {
                _treatmentService.DeletePlanEntry(entry_id);
                return Results.Ok();        //to say it did well
            }
            catch (HospitalException ex) { return Results.NotFound(ex.Message); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        }
    }

    public record NewPlanRequest(
            int Patient_ID,
            int Staff_ID,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation
        );

    public record NewEntryRequest(
            int Plan_ID,
            int Staff_ID,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            int? Drug_ID,
            string? Posology,
            string? Upload_Link
        );

    public record UpdatePlanRequest(
            int Plan_ID,
            int Patient_ID,
            int Staff_ID,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation
        );

    public record UpdateEntryRequest(
            int Entry_ID,
            int Plan_ID,
            int Staff_ID,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            int? Drug_ID,
            string? Posology,
            string? Upload_Link
        );
}
