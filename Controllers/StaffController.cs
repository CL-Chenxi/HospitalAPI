using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private DatabaseContext _db = new DatabaseContext();  // why it gives out about the _db, what's _db represent here?
                                                              //_db is databse objects, through this object, can access PatientSet
                                                              //_db is a var name, we just named
                                                              //this is a property

        [HttpGet("GetStaffById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Patient))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]

        public IResult GetStaffById(int staff_id)           //IResult -->interfeaceResult, a super type for represent anything
        {
            var result = _db.PatientSet.Find(staff_id);       //Result is find data and send back to Swagger,
                                                        //Result is data order that contains info. found in DB/error msg
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        
    }
}
