using HospitalAPI.Controllers;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;

namespace HospitalAPI.services
{
    //The purpose of the Service layer is to provide insulation between Controllers and the database.
    //It is generally bad practice to have controllers directly query the DB.
    //The Service can also house any transformative logic to apply to the DB query result before returning it to the user
    public class PatientService
    {
        private DatabaseContext _db;

        public PatientService() 
        {
            _db = new DatabaseContext();
        }

        public PatientService(DatabaseContext db)
        {
            _db = db;
        }

        public PatientDto? GetPatientById(int PatientId)
        {
            var result = _db.PatientSet.Find(PatientId);
            if (result == null) { return null; }
            else return MapFrom(result);
        }

        public List<PatientDto> GetAllPatients() 
        {
            if(_db.PatientSet.IsNullOrEmpty()) return new List<PatientDto>();
            else return _db.PatientSet.ToList().ConvertAll(MapFrom);
        }

        public List<PatientDto> SearchPatient(SearchPatientRequest SearchParams)
        {
            List<PatientDto> patientList = new List<PatientDto>();
            //if there's an Id provided, let's use that first
            if (SearchParams.Patient_Id.HasValue)
            {
                //note this is how a nullable (e.g 'int?') works: use .HasValue to check if null or not,
                // then use .Value to get to the value (as below)
                var unique = GetPatientById(SearchParams.Patient_Id.Value);
                if (unique == null) return patientList; //return empty list
                else patientList.Add(unique);

            }
            else
            {
                //if no Id is provided, we check if we are provided with a first name, a last name, or both. If not, replace with wildcard (*)
                string fName;
                string lName;
                if (string.IsNullOrEmpty(SearchParams.First_Name))
                { fName = "*"; }
                else { fName = SearchParams.First_Name; }
                if (string.IsNullOrEmpty(SearchParams.Last_Name))
                { lName = "*"; }
                else { lName = SearchParams.Last_Name; }

                patientList = _db.PatientSet.Where(p =>
                        (p.Patient_lName == lName || lName == "*") &&
                        (p.Patient_fName == fName || fName == "*")
                    ).ToList().ConvertAll(MapFrom);
            }
            return patientList;
        }

        public PatientDto CreateNewPatient(NewPatientRequest patientRequest)
        {
            Patient patient = new Patient();
            patient.Patient_fName = patientRequest.Patient_fName;
            patient.Patient_lName = patientRequest.Patient_lName;
            patient.Patient_DoB = patientRequest.Patient_DoB;
            patient.Patient_PhoneNum = patientRequest.Patient_PhoneNum;
            patient.Patient_Allergy = patientRequest.Patient_Allergy;
                
            EntityEntry<Patient> newPatient = _db.PatientSet.Add(patient); //EntityEntry is a container for the newly saved Patient
            _db.SaveChanges();
            return MapFrom(newPatient.Entity); //so we can have the Patient Id we just created (useful to the front end)
        }

        public void UpdatePatient(UpdatePatientRequest patientDto)
        {
            var result = _db.PatientSet.Find(patientDto.Patient_ID);
            if (result == null) 
            {
                //throw exception to be processed at controller level
                throw new HospitalException("Patient Id not found");
            }
            else 
            {
                Patient patient = result;
                patient.Patient_fName = patientDto.Patient_fName;
                patient.Patient_lName= patientDto.Patient_lName;
                patient.Patient_DoB = patientDto.Patient_DoB;
                patient.Patient_PhoneNum = patientDto.Patient_PhoneNum;
                patient.Patient_Allergy = patientDto.Patient_Allergy;
                _db.PatientSet.Update(patient);
                _db.SaveChanges();
            }
        }

        public void DeletePatient(int patientId) 
        {
            var result = _db.PatientSet.Find(patientId);
            if (result == null)
            {
                //throw exception to be processed at controller level
                throw new HospitalException("Patient Id not found");
            }
            else 
            {
                //delete related treatment plans and entries (cascade delete)
                Patient patient = result;
                List<TreatmentPlan> planList = _db.TreatmentPlanSet.Where(p => p.Patient_ID == patientId).ToList();
                foreach (TreatmentPlan plan in planList)
                {
                    _db.TreatmentPlanEntrySet.Where(entry => plan.Plan_ID == entry.Plan_ID).ExecuteDelete();
                    _db.TreatmentPlanSet.Remove(plan);
                }
                _db.PatientSet.Remove(patient);
                _db.SaveChanges();
            }
        }


        public PatientDto MapFrom(Patient PatientEntity)
        {
            return new PatientDto(
                PatientEntity.Patient_ID,
                PatientEntity.Patient_fName,
                PatientEntity.Patient_lName,
                PatientEntity.Patient_DoB,
                PatientEntity.Patient_PhoneNum,
                PatientEntity.Patient_Allergy
                );
        }
    }

    //this class is used as a return value. Records are like mini-classes with simplified declaration 
    public record PatientDto(
        int Patient_ID,
        string Patient_fName,
        string Patient_lName,
        DateOnly Patient_DoB,
        string Patient_PhoneNum,
        string Patient_Allergy
        );
}
