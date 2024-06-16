using HospitalAPI.Controllers;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata.Ecma335;

namespace HospitalAPI.services
{
    //The purpose of the Service layer is to provide insulation between Controllers and the database.
    //It is generally bad practice to have controllers directly query the DB.
    //The Service can also house any transformative logic to apply to the DB query result before returning it to the user
    public class TreatmentService
    {
        private DatabaseContext _db = new DatabaseContext();
        private StaffService _staffService = new StaffService();
        private PatientService _patientService = new PatientService();
        private DrugService _drugService = new DrugService();

        public static List<string> Status = new List<string> { "OPEN", "ONGOING", "CLOSED", "SUSPENDED" };
        public static List<string> Type = new List<string> { "BLOOD", "PRESCRIPTION", "MRI", "SCAN", "XRAY" };
        public TreatmentPlanDto? GetTreatmentPlanById(int PlanId)
        {
            var result = _db.TreatmentPlanSet.Find(PlanId);
            if(result == null) return null;
            else
            {
                TreatmentPlan tp = result;
                return MapFrom(tp);
            }
        }

        public List<TreatmentPlanDto> GetAllTreatmentPlansForPatient(int PatientId)
        {
            if (_db.PatientSet.Find(PatientId) == null) 
            {
                throw new HospitalException();//if patient doesn't exist, manage this at controller level
            } 
            else return _db.TreatmentPlanSet //is a list, because, a patient can have multiple plans
                .Where(t => t.Patient_ID == PatientId) //entries must match patient id 
                .OrderByDescending(t => t.Plan_Date)
                .ToList()       //transform into  list
                .ConvertAll(MapFrom);
        }

        public TreatmentPlanDto CreateNewTreatmentPlan(NewPlanRequest PlanRequest)
        {
            // check those Id exists
            if (_db.PatientSet.Find(PlanRequest.Patient_ID) == null)
            {
                throw new HospitalException("Patient Id #" + PlanRequest.Patient_ID + " not found");
            }
            Staff? staff = _db.StaffSet.Find(PlanRequest.Staff_ID);
            if (staff == null) throw new HospitalException("Staff Id #" + PlanRequest.Staff_ID + " not found");
            if (!staff.Staff_Active) throw new HospitalException("Staff Id #" + PlanRequest.Staff_ID + " is inactive");
            if (!Status.Contains(PlanRequest.Plan_Status.ToUpper())) throw new HospitalException("Status type not found");
            TreatmentPlan planEntity = new TreatmentPlan();
            planEntity.Staff_ID = PlanRequest.Staff_ID;
            planEntity.Patient_ID = PlanRequest.Patient_ID;
            planEntity.Plan_CycleLen = PlanRequest.Plan_Cycle;
            planEntity.Plan_Status = PlanRequest.Plan_Status.ToUpper();
            planEntity.Plan_Date = PlanRequest.Plan_Date;
            planEntity.Plan_Observation = PlanRequest.Plan_Observation;
            EntityEntry<TreatmentPlan> newPlan = _db.TreatmentPlanSet.Add(planEntity); //EntityEntry is a container for the newly created plan
            _db.SaveChanges();
            return MapFrom(newPlan.Entity);
        }

        public void UpdateTreatmentPlan(UpdatePlanRequest PlanRequest)
        {
            var planEntity = _db.TreatmentPlanSet.Find(PlanRequest.Plan_ID);
            if (planEntity == null) throw new HospitalException("Plan Id #"+PlanRequest.Plan_ID+" not found");
            // check those Id exists
            if (_db.PatientSet.Find(PlanRequest.Patient_ID) == null)
            {
                throw new HospitalException("Patient Id #" + PlanRequest.Patient_ID + " not found");
            }
            Staff? staff = _db.StaffSet.Find(PlanRequest.Staff_ID);
            if (staff == null) throw new HospitalException("Staff Id #" + PlanRequest.Staff_ID + " not found");
            if (!staff.Staff_Active && PlanRequest.Staff_ID != planEntity.Staff_ID) throw new HospitalException("Staff Id #" + PlanRequest.Staff_ID + " is inactive");

            if (!Status.Contains(PlanRequest.Plan_Status.ToUpper())) throw new HospitalException("Status type not found");
            //update what makes sense
            planEntity.Plan_Date = PlanRequest.Plan_Date;
            planEntity.Plan_Status = PlanRequest.Plan_Status.ToUpper();
            planEntity.Plan_Observation = PlanRequest.Plan_Observation;
            planEntity.Plan_CycleLen = PlanRequest.Plan_Cycle;
            _db.TreatmentPlanSet.Update(planEntity);
            _db.SaveChanges();
        }

        public void DeleteTreatmentPlan(int PlanId)
        {
            var planEntity = _db.TreatmentPlanSet.Find(PlanId);
            if (planEntity == null) throw new HospitalException("Plan Id not found");
            
            _db.TreatmentPlanEntrySet.Where(e => e.Plan_ID == PlanId).ExecuteDelete();
            _db.TreatmentPlanSet.Remove(planEntity); 
            _db.SaveChanges();
        }

        //below is for Plan entries

        public TreatmentEntryDto? GetTreatmentPlanEntryById(int EntryId) 
        { 
            var result = _db.TreatmentPlanEntrySet.Find(EntryId);
            if(result == null) return null;
            else return MapFrom(result);
        }

        public List<TreatmentEntryDto> GetAllPlanEntriesForPlanId(int PlanId)
        {
            if (_db.TreatmentPlanSet.Find(PlanId) == null) return new List<TreatmentEntryDto>();
            else return _db.TreatmentPlanEntrySet
                .Where(t => t.Plan_ID == PlanId)
                .OrderByDescending(t => t.Entry_ID)
                .ToList()
                .ConvertAll(MapFrom);
        }

        public TreatmentEntryDto CreateNewEntry(NewEntryRequest EntryRequest)
        {
            //checks all ids involved exists
            if (_db.TreatmentPlanSet.Find(EntryRequest.Plan_ID) == null) throw new HospitalException("Plan Id not found");

            Staff? staff = _db.StaffSet.Find(EntryRequest.Staff_ID);
            if(staff == null) throw new HospitalException("Staff Id #"+EntryRequest.Staff_ID+" not found");
            if(!staff.Staff_Active) throw new HospitalException("Staff Id #" + EntryRequest.Staff_ID + " is inactive");
            if (EntryRequest.Drug_ID.HasValue)
            {
                Drug? drug = _db.DrugSet.Find(EntryRequest.Drug_ID.Value);
                if (drug == null) throw new HospitalException("Drug ID #"+ EntryRequest.Drug_ID.Value + " not found");
                if (drug.Drug_Available == false) throw new HospitalException("Drug ID #" + EntryRequest.Drug_ID.Value + " is unavailable");
            }

            if(!Type.Contains(EntryRequest.Entry_Type.ToUpper())) throw new HospitalException("Entry type not found");

            TreatmentPlanEntry entryEntity = new TreatmentPlanEntry();
            entryEntity.Plan_ID = EntryRequest.Plan_ID;
            entryEntity.Staff_ID = EntryRequest.Staff_ID;
            entryEntity.Last_Update = EntryRequest.Last_Update;
            entryEntity.Entry_Type = EntryRequest.Entry_Type.ToUpper();
            entryEntity.Comment = EntryRequest.Comment; 
            entryEntity.Drug_ID = EntryRequest.Drug_ID;
            entryEntity.Posology = EntryRequest.Posology;
            entryEntity.UploadLink = EntryRequest.Upload_Link;

            EntityEntry<TreatmentPlanEntry> newPlanEntry = _db.TreatmentPlanEntrySet.Add(entryEntity); 
            _db.SaveChanges();
            return MapFrom(newPlanEntry.Entity);
        }

        public void UpdatePlanEntry(UpdateEntryRequest EntryRequest)
        {
            var entryEntity = _db.TreatmentPlanEntrySet.Find(EntryRequest.Entry_ID);
            if (entryEntity == null) throw new HospitalException("Plan entry Id not Found");

            Staff? staff = _db.StaffSet.Find(EntryRequest.Staff_ID);
            if (staff == null) throw new HospitalException("Staff Id #" + EntryRequest.Staff_ID + " not found");
            if (!staff.Staff_Active && entryEntity.Staff_ID != EntryRequest.Staff_ID) throw new HospitalException("Staff Id #" + EntryRequest.Staff_ID + " is inactive");
            if (EntryRequest.Drug_ID.HasValue)
            {
                Drug? drug = _db.DrugSet.Find(EntryRequest.Drug_ID.Value);
                if (drug == null) throw new HospitalException("Drug ID #" + EntryRequest.Drug_ID.Value + " not found");
                if (!drug.Drug_Available && entryEntity.Drug_ID != EntryRequest.Drug_ID) throw new HospitalException("Drug ID #" + EntryRequest.Drug_ID.Value + " is unavailable");
            }
            if (!Type.Contains(EntryRequest.Entry_Type.ToUpper())) throw new HospitalException("Entry type not found");

            entryEntity.Staff_ID = EntryRequest.Staff_ID;
            entryEntity.UploadLink = EntryRequest.Upload_Link;
            entryEntity.Comment = EntryRequest.Comment;
            entryEntity.Last_Update = EntryRequest.Last_Update;
            entryEntity.Drug_ID = EntryRequest.Drug_ID;
            entryEntity.Posology = EntryRequest.Posology;
            _db.TreatmentPlanEntrySet.Update(entryEntity);
            _db.SaveChanges();
        }

        public void DeletePlanEntry(int EntryID)
        {
            var entryEntity = _db.TreatmentPlanEntrySet.Find(EntryID);
            if (entryEntity == null) throw new HospitalException("Plan entry Id not Found");
            _db.TreatmentPlanEntrySet.Remove(entryEntity);
            _db.SaveChanges();
        }


        //mappers

        public TreatmentPlanDto MapFrom(TreatmentPlan planEntity)
        {
            StaffDto? staff = _staffService.GetStaffById(planEntity.Staff_ID);
            PatientDto? patient = _patientService.GetPatientById(planEntity.Patient_ID);
            if (staff == null || patient == null) { throw new HospitalException("Incoherent values retrieved from Database"); }
            List<TreatmentEntryDto> entriesList = GetAllPlanEntriesForPlanId(planEntity.Plan_ID);
            return new TreatmentPlanDto(
                planEntity.Plan_ID,
                patient,
                staff,
                planEntity.Plan_CycleLen,
                planEntity.Plan_Status,
                planEntity.Plan_Date,
                planEntity.Plan_Observation,
                entriesList
                );
        }

        public TreatmentEntryDto MapFrom(TreatmentPlanEntry entryEntity)
        {
            StaffDto? staff = _staffService.GetStaffById(entryEntity.Staff_ID);
            if (staff == null) { throw new HospitalException("Incoherent staff Id retrieved from Database");  }
            DrugDto? drug;
            if (entryEntity.Drug_ID.HasValue)
            {
                drug = _drugService.GetDrugById(entryEntity.Drug_ID.Value);
                if (drug == null) throw new HospitalException("Incoherent drug Id retrieved from Database");
            }
            else drug = null;
             
            return new TreatmentEntryDto(
                entryEntity.Entry_ID,
                entryEntity.Plan_ID,
                staff,
                entryEntity.Last_Update,
                entryEntity.Entry_Type,
                entryEntity.Comment,
                drug,
                entryEntity.Posology,
                entryEntity.UploadLink
                );
        }
    }

    //those class is used as a return value. Records are like mini-classes with simplified declaration 
    //this is a good example of why we're using a DTO and not directly the class from the DB: we can change it form to hold more data
    //than there is in the TreatmentPlan table, by collating information from Patient, Drug, and Staff table.
    public record TreatmentPlanDto(
            int Plan_ID,
            PatientDto patient,
            StaffDto staff,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation,
            List<TreatmentEntryDto> Plan_entries
        );

    public record TreatmentEntryDto(
            int Entry_ID,
            int Plan_ID,
            StaffDto Staff,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            DrugDto? Drug,
            string? Posology,
            string? Upload_Link
        );

}
