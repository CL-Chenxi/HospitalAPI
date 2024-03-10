using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations; // for all Swagger Annotations

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                            //_db is databse objects, through this object, can access PatientSet
                                                            //_db is a var name, we just named
                                                            //this is a property

        [HttpGet("GetPatientById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Patient))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult GetPatientById(int patient_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            var result = _db.PatientSet.Find(patient_id);       //Result is find data and send back to Swagger,
                                                        //Result is data order that contains info. found in DB/error msg
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        [HttpPost("CreatePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]

        public IResult Create([FromBody] Patient patient)     //FromBody means body of request, contains my request creteria,
                                                                        //create contains all info we need for create 
                                                                        //map the body content over patient object
                                                                        //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (patient == null) 
            {
                return Results.BadRequest();
            }
            _db.PatientSet.Add(patient);        //set is like a working progress
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdatePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]
        
        public IResult Update(int patient_id, [FromBody] Patient patient)
        {
            var result = _db.PatientSet.Find(patient_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                result.Patient_fName = patient.Patient_fName;
                result.Patient_lName = patient.Patient_lName;
                result.Patient_DoB = patient.Patient_DoB;
                result.Patient_PhoneNum = patient.Patient_PhoneNum;
                result.Patient_Allergy = patient.Patient_Allergy;

                _db.PatientSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();        //to say it did well
        }

        [HttpDelete("DeletePatient")]
        [SwaggerResponse(StatusCodes.Status200OK, "Patient Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult Delete(int patient_id)
        {
            var result = _db.PatientSet.Find(patient_id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.PatientSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }


    }
}