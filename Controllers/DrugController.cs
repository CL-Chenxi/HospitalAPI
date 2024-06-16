using HospitalAPI.Models;
using HospitalAPI.services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class DrugController
    {
        private DrugService _drugService = new DrugService();

        [HttpGet("GetDrugById")]
        public IResult GetDrugById(int drug_id)
        {

            var result = _drugService.GetDrugById(drug_id);
            if (result == null) { return Results.NotFound("Drug ID #" + drug_id + " not found"); }
            return Results.Ok(result);
        }

        [HttpGet("GetAllDrugs")]
        public IResult GetDrugList()
        {
            return Results.Ok(_drugService.GetDrugList());
        }

        [HttpPost("AddDrug")]
        public IResult AddNewDrug([FromBody] NewDrugRequest drug) 
        {
            if(drug != null)
            {
                try
                {
                    return Results.Ok(_drugService.AddNewDrug(drug));
                } catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
                
            } 
            return Results.NoContent();
        }

        [HttpPut("UpdateDrug")]
        public IResult UpdateDrug([FromBody] UpdateDrugRequest drug)
        {
            if (drug != null)
            {
                try
                {
                    return Results.Ok(_drugService.UpdateDrug(drug));
                } catch (HospitalException ex)
                {
                    return Results.NotFound(ex.Message);
                } 
            }
            return Results.NoContent();
        }

        [HttpPut("DeactivateDrug")]
        public IResult DeactivateDrug(int drug_id)
        {
            try
            {
                _drugService.DeactivateDrug(drug_id);
                return Results.Ok();
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
        [HttpPut("ReactivateDrug")]
        public IResult ReactivateDrug(int drug_id)
        {
            try
            {
                _drugService.ReactivateDrug(drug_id);
                return Results.Ok();
            }
            catch (HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteDrug")]
        public IResult DeleteDrug(int drug_id)
        {
            try
            {
                _drugService.Delete(drug_id);
                return Results.Ok();
            } catch(HospitalException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
    }

    public record NewDrugRequest
        (
        string Drug_Name,
        string Drug_Dosage,
        string Drug_AllergyList
        );

    public record UpdateDrugRequest
    (
            int Drug_ID,
            string Drug_Name,
            string Drug_Dosage,
            string Drug_AllergyList
    );
}
