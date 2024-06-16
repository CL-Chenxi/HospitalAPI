 using Microsoft.AspNetCore.Mvc;
using HospitalAPI.Models;
using Microsoft.IdentityModel.Tokens;
using HospitalAPI.services;

namespace HospitalAPI.Controllers
{
    //Using a database intializer would be a way more elegant solution, but I don't have time to solve all the compatibility issue
    [ApiController]     //tell any front end browser where the end point to make request form API Controller
    [Route("api/[controller]")]
    public class DatabaseController
    {
        private DatabaseContext _db = new DatabaseContext();

        [HttpGet("GetStatusList")]
        public IResult GetStatusList()
        {
            return Results.Ok(TreatmentService.Status);
        }
        [HttpGet("GetEntryTypeList")]
        public IResult GetEntryTypeList()
        {
            return Results.Ok(TreatmentService.Type);
        }


        [HttpPost("initiate")]
        public void initiate()
        {
            if(_db.DrugSet.IsNullOrEmpty()) //only if those tables are empty fill them up
            {
                SetupDrugs();
            }
            if(_db.StaffSet.IsNullOrEmpty())
            {
                SetupStaff();
            }
        }

        private void SetupDrugs()
        {
            var csv = System.IO.File.ReadAllText("./Resources/InitialDrugs.csv");
            string[] lines = csv.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                Drug drug = new Drug();
                string[] elms = line.Split(",");
                drug.Drug_Name = elms[0];
                drug.Drug_Dosage = elms[1];
                drug.Drug_AllergyList = elms[2];
                drug.Drug_Available = true;
                _db.DrugSet.Add(drug);
            }
            _db.SaveChanges();
        }

        private void SetupStaff()
        {
            var csv = System.IO.File.ReadAllText("./Resources/InitialStaff.csv");
            string[] lines = csv.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                Staff staff = new Staff();
                string[] elms = line.Split(",");
                staff.Staff_fName = elms[0];
                staff.Staff_lName = elms[1];
                staff.Staff_PhoneNum = elms[2];
                staff.Staff_Grade = int.Parse(elms[3]);
                staff.Staff_Active = true;
                _db.StaffSet.Add(staff);
            }
            _db.SaveChanges();
        }

    }
}
