using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class TestResultController : Controller
    {


        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                              //_db is databse objects, through this object, can access PatientSet
                                                              //_db is a var name, we just named
                                                              //this is a property

        [HttpGet("GetPatientById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TestResult))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Patient ID Not Found")]

        public IResult GetTestResultById(int testresult_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            var testresult = _db.PatientSet.Find(testresult_id);       //Result is find data and send back to Swagger,
                                                                       //Result is data order that contains info. found in DB/error msg
            if (testresult == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(testresult);
        }

        [HttpPut("CreateTestResult")]
        [SwaggerResponse(StatusCodes.Status200OK, "TestResult Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]

        public IResult Create([FromBody] TestResult testresult)     //FromBody means body of request, contains my request creteria,
                                                                    //create contains all info we need for create 
                                                                    //map the body content over patient object
                                                                    //Patient is object, patient can be called mypatient, is a name of the object
        {
            if (testresult == null)
            {
                return Results.BadRequest();
            }
            _db.TestResultSet.Add(testresult);        //set is like a working progress
            _db.SaveChanges();
            return Results.Ok();
        }


        [HttpPut("UpdateTestResult")]
        [SwaggerResponse(StatusCodes.Status200OK, "TestResult Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input Value Incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "TestResult ID Not Found")]

        public IResult Update(int testresult_id, [FromBody] TestResult testresult) //trest is being retrived,updated with value from testresult
                                                                                   // to update trest, and save the trsult to TestResultSet

        {
            var tresult = _db.TestResultSet.Find(testresult_id);
            if (tresult == null)
            {
                return Results.NotFound();
            }
            else
            {
                tresult.TestRes_Date = testresult.TestRes_Date;
                tresult.TestRes_Type = testresult.TestRes_Type;
                tresult.TestRes_Link = testresult.TestRes_Link;
                tresult.TestRes_Observation = testresult.TestRes_Observation;


                _db.TestResultSet.Update(tresult);
                _db.SaveChanges();
            }
            return Results.Ok();        //to say it did well
        }

        [HttpDelete("DeleteTestResult")]
        [SwaggerResponse(StatusCodes.Status200OK, "Test Result Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized Client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Test Result ID Not Found")]

        public IResult Delete(int testresult_id)
        {
            var tresult = _db.TestResultSet.Find(testresult_id);
            if (tresult == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.TestResultSet.Remove(tresult);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
