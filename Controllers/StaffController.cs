using HospitalAPI.Models;
using HospitalAPI.services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private StaffService _StaffService = new StaffService();

        [HttpGet("GetStaffById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StaffDto))]      //convience premade msg 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]
        public IResult GetStaffById(int staff_id)           //IResult -->interface Result, a super type for represent anything
        {
            var result = _StaffService.GetStaffById(staff_id);       //Result is find data and send back to Swagger,
                                                                     //Result is data order that contains info. found in DB/error msg
            if (result == null)
            {
                return Results.NotFound("Staff ID #" + staff_id + " not found");
            }
            return Results.Ok(result);
        }

        [HttpGet("GetStaffList")]
        public IResult GetStaffList()
        {
            return Results.Ok(_StaffService.GetStaffList());

        }

        [HttpGet("SearchStaff")]
        public IResult GetStaffByName([FromQuery] SearchStaffRequest searchParams)
        {
            return Results.Ok(_StaffService.SearchStaff(searchParams));
        }

        [HttpPost("AddStaff")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StaffDto))]
        public IResult AddStaff([FromBody] NewStaffRequest NewStaff)
        {
            return Results.Ok(_StaffService.AddNewStaff(NewStaff));
        }

        [HttpPut("UpdateStaff")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StaffDto))] 
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]
        public IResult UpdateStaff([FromBody] UpdateStaffRequest UpdateStaff)
        {
            if (UpdateStaff == null) return Results.BadRequest();
            try
            {
                return Results.Ok(_StaffService.UpdateStaff(UpdateStaff));
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteStaff")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]
        public IResult DeleteStaff(int staff_id)
        {
            try
            {
                _StaffService.DeleteStaff(staff_id);
                return Results.Ok();
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        [HttpPut("DeactivateStaff")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]
        public IResult DeactivateStaff(int staff_id)
        {
            try
            {
                _StaffService.DeactivateStaff(staff_id);
                return Results.Ok();
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        [HttpPut("ReactivateStaff")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Staff ID Not Found")]
        public IResult ReactivateStaff(int staff_id)
        {
            try
            {
                _StaffService.ReactivateStaff(staff_id);
                return Results.Ok();
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
    }

    public class SearchStaffRequest
    {
        [AllowNull]
        public int? Staff_Id { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? First_Name { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? Last_Name { get; set; }
    }

    public record NewStaffRequest
        (
            string Staff_fName,
            string Staff_lName,
            string Staff_Phone,
            int Staff_Grade
        );

    public record UpdateStaffRequest
        (
            int Staff_ID,
            string Staff_fName,
            string Staff_lName,
            string Staff_Phone,
            int Staff_Grade
        );
}
