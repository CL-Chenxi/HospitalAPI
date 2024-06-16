using HospitalAPI.Models;
using HospitalAPI.services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis; // for all Swagger Annotations

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {

        private PatientService _patientService = new PatientService();

        [HttpGet("GetPatientById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PatientDto))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult GetPatientById(int patient_id)           //IResult -->interface Result, a super type for represent anything
        {
            var result = _patientService.GetPatientById(patient_id);       //Result is find data and send back to Swagger,
                                                        //Result is data order that contains info. found in DB/error msg
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        //combine search by id and search by first / last name. Leave all fields empty to get the whole patient list
        [HttpGet("SearchPatient")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<PatientDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value missing or incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        public IResult SearchPatient([FromQuery] SearchPatientRequest parametersDto) //GET methods don't have a body, so use 'fromQuery' instead
        {
            if (parametersDto == null) { return Results.BadRequest(); }
            
            return Results.Ok(_patientService.SearchPatient(parametersDto));
        }

        [HttpGet("GetPatientList")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<PatientDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value missing or incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        public IResult GetPatientList()
        {
            return Results.Ok(_patientService.GetAllPatients());
        }

        [HttpPost("CreatePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult Create([FromBody] NewPatientRequest patientRequest)     //FromBody means body of request, contains my request criteria,
                                                                        //create contains all info we need for create 
                                                                        //map the body content over patient object
                                                                        //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (patientRequest == null) 
            {
                return Results.BadRequest();
            }
            try
            {
                PatientDto newPatient = _patientService.CreateNewPatient(patientRequest);
                return Results.Ok(newPatient);
            } catch (HospitalException)
            { 
                return Results.NotFound();
            }
            catch (Exception ex)
            { 
                return Results.BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdatePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]
        
        public IResult Update([FromBody] UpdatePatientRequest patient)
        {
            if (patient == null)
            {
                return Results.BadRequest();
            } 
            try
            {
                _patientService.UpdatePatient(patient);
                return Results.Ok();        //to say it did well
            }catch (HospitalException)
            {
                return Results.NotFound();
            } 
            catch (Exception ex) 
            {
                return Results.BadRequest(ex.Message);
            }

        }

        [HttpDelete("DeletePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult Delete(int patient_id)
        {
            try
            {
                _patientService.DeletePatient(patient_id);
                return Results.Ok();
            }
            catch (HospitalException)
            {
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }


    }

    public class SearchPatientRequest //object dedicated to receive search requests parameters from the front end
    {
        [AllowNull]
        public int? Patient_Id { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? First_Name { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? Last_Name { get; set; }
    }

    public record NewPatientRequest(
        string Patient_fName,
        string Patient_lName,
        DateOnly Patient_DoB,
        string Patient_PhoneNum,
        string Patient_Allergy
        );
    //object received from the front end containing all info to create a new patient in the DB

    public record UpdatePatientRequest(
        int Patient_ID,
        string Patient_fName,
        string Patient_lName,
        DateOnly Patient_DoB,
        string Patient_PhoneNum,
        string Patient_Allergy
    );
}