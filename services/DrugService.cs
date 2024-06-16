using HospitalAPI.Controllers;
using HospitalAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace HospitalAPI.services
{
    //The purpose of the Service layer is to provide insulation between Controllers and the database.
    //It is generally bad practice to have controllers directly query the DB.
    //The Service can also house any transformative logic to apply to the DB query result before returning it to the user
    public class DrugService
    {
        private DatabaseContext _db = new DatabaseContext();

        public DrugDto? GetDrugById(int DrugId)
        {
            var result = _db.DrugSet.Find(DrugId);
            if (result == null) { return null; }
            else return MapFrom(result);
        }

        public List<DrugDto> GetDrugList() 
        {
            if(_db.DrugSet.IsNullOrEmpty()) 
            { 
                return new List<DrugDto>(); 
            }
            return _db.DrugSet.ToList().ConvertAll(MapFrom);
        }

        public DrugDto AddNewDrug(NewDrugRequest NewDrug)
        {
            Drug drug = new Drug();
            drug.Drug_Name = NewDrug.Drug_Name;
            drug.Drug_Dosage = NewDrug.Drug_Dosage;
            drug.Drug_AllergyList = NewDrug.Drug_AllergyList;
            drug.Drug_Available = true;
            _db.DrugSet.Add(drug);
            _db.SaveChanges();
            return MapFrom(drug);
        }

        public DrugDto UpdateDrug(UpdateDrugRequest UpdateDrug)
        {
            Drug? drug = _db.DrugSet.Find(UpdateDrug.Drug_ID);
            if (drug == null) { throw new HospitalException("Drug ID #" + UpdateDrug.Drug_ID + " not found"); }
            drug.Drug_Name = UpdateDrug.Drug_Name;
            drug.Drug_Dosage = UpdateDrug.Drug_Dosage;
            drug.Drug_AllergyList = UpdateDrug?.Drug_AllergyList;
            _db.DrugSet.Update(drug);
            _db.SaveChanges();
            return MapFrom(drug);
        }

        public void DeactivateDrug(int DrugId)
        {
            Drug? drug = _db.DrugSet.Find(DrugId);
            if (drug == null) { throw new HospitalException("Drug ID #"+DrugId+" not found");  }
            drug.Drug_Available = false;
            _db.DrugSet.Update(drug);
            _db.SaveChanges();
        }

        public void ReactivateDrug(int DrugId)
        {
            Drug? drug = _db.DrugSet.Find(DrugId);
            if (drug == null) { throw new HospitalException("Drug ID #" + DrugId + " not found"); }
            drug.Drug_Available = true;
            _db.DrugSet.Update(drug);
            _db.SaveChanges();
        }

        public void Delete(int DrugId)
        {
            Drug? drug = _db.DrugSet.Find(DrugId);
            if (drug == null) { throw new HospitalException("Drug ID #" + DrugId + " not found"); }
            if(_db.TreatmentPlanEntrySet.Where(entry => entry.Drug_ID == DrugId).Any()) //check that it's not used anywhere
            {
                throw new HospitalException("Drug ID #" + DrugId + " can only be deactivated not deleted");
            } else
            {
                _db.DrugSet.Remove(drug);
                _db.SaveChanges();
            }
            
        }

        public DrugDto MapFrom(Drug DrugEntity)
        {
            return new DrugDto(
                    DrugEntity.Drug_ID,
                    DrugEntity.Drug_Name,
                    DrugEntity.Drug_Dosage,
                    DrugEntity.Drug_AllergyList, //split?
                    DrugEntity.Drug_Available
                );
        }

    }

    public record DrugDto(
            int Drug_ID,
            string Drug_Name,
            string Drug_Dosage,
            string Drug_AllergyList,
            Boolean Drug_Available
        );
}
