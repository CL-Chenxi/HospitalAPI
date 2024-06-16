using HospitalAPI.Controllers;
using HospitalAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace HospitalAPI.services
{
    //The purpose of the Service layer is to provide insulation between Controllers and the database.
    //It is generally bad practice to have controllers directly query the DB.
    //The Service can also house any transformative logic to apply to the DB query result before returning it to the user
    public class StaffService
    {
        private DatabaseContext _db = new DatabaseContext();

        public StaffDto? GetStaffById(int staffId)
        {
            var result = _db.StaffSet.Find(staffId);
            if (result == null)
            {
                return null;
            } else
            {
                return MapFrom(result);
            } 

        }

        public List<StaffDto> SearchStaff(SearchStaffRequest searchParameters)
        {
            List<StaffDto> list = new List<StaffDto>();
            if (searchParameters == null) { return GetStaffList(); }
            if (searchParameters.Staff_Id.HasValue)
            {
                var res = GetStaffById(searchParameters.Staff_Id.Value);
                if (res == null) { return list; }
                else list.Add(res);
            }
            else
            {
                string fName;
                string lName;
                if (string.IsNullOrEmpty(searchParameters.First_Name))
                { fName = "*"; }
                else { fName = searchParameters.First_Name; }
                if (string.IsNullOrEmpty(searchParameters.Last_Name))
                { lName = "*"; }
                else { lName = searchParameters.Last_Name; }
                list = _db.StaffSet.Where(p =>
                    (p.Staff_lName == lName || lName == "*") &&
                    (p.Staff_fName == fName || fName == "*")).ToList().ConvertAll(MapFrom);
            }
            return list;
        }

        public List<StaffDto> GetStaffList() 
        {
            if(_db.StaffSet.IsNullOrEmpty()) return new List<StaffDto>();
            else return _db.StaffSet.ToList().ConvertAll(MapFrom); // shortend to convert the whole list into StaffDto
        }

        public StaffDto AddNewStaff(NewStaffRequest NewStaff)
        {
            Staff staff = new Staff();
            staff.Staff_fName = NewStaff.Staff_fName;
            staff.Staff_lName = NewStaff.Staff_lName;
            staff.Staff_PhoneNum = NewStaff.Staff_Phone;
            staff.Staff_Grade = NewStaff.Staff_Grade;
            staff.Staff_Active = true;
            _db.StaffSet.Add(staff);
            _db.SaveChanges();
            return MapFrom(staff);
        }

        public StaffDto UpdateStaff(UpdateStaffRequest UpdateStaff)
        {
            Staff? staff = _db.StaffSet.Find(UpdateStaff.Staff_ID);
            if(staff == null) throw new HospitalException("Staff ID #" + UpdateStaff.Staff_ID + " not found");
            staff.Staff_fName = UpdateStaff.Staff_fName;
            staff.Staff_lName = UpdateStaff.Staff_lName;
            staff.Staff_PhoneNum = UpdateStaff.Staff_Phone;
            staff.Staff_Grade = UpdateStaff.Staff_Grade;
            _db.StaffSet.Update(staff);
            _db.SaveChanges();
            return MapFrom(staff);
        }

        public void DeactivateStaff(int StaffID)
        {
            Staff? staff = _db.StaffSet.Find(StaffID);
            if (staff == null) throw new HospitalException("Staff ID #" + StaffID + " not found");
            staff.Staff_Active = false;
            _db.StaffSet.Update(staff);
            _db.SaveChanges();
        }

        public void ReactivateStaff(int StaffID)
        {
            Staff? staff = _db.StaffSet.Find(StaffID);
            if (staff == null) throw new HospitalException("Staff ID #" + StaffID + " not found");
            staff.Staff_Active = true;
            _db.StaffSet.Update(staff);
            _db.SaveChanges();
        }

        public void DeleteStaff(int StaffID)
        {
            Staff? staff = _db.StaffSet.Find(StaffID);
            if (staff == null) throw new HospitalException("Staff ID #"+StaffID+" not found");
            if(_db.TreatmentPlanSet.Where(plan => plan.Staff_ID == StaffID).Any() || 
                _db.TreatmentPlanEntrySet.Where(entry => entry.Staff_ID == StaffID).Any()) //check that it's not used anywhere
            {
                throw new HospitalException("Staff ID #"+StaffID+" can only be deactivated, not deleted");
            } else
            {
                _db.StaffSet.Remove(staff);
                _db.SaveChanges();
            }

        }

        //convert a Staff entity from the DB into a returnable object.
        //Useful if one day you want to return more info than what is present in the DB, or hide some info from the DB you don't want to see
        public StaffDto MapFrom(Staff staffEntity)
        {
            return new StaffDto(
                staffEntity.Staff_ID,
                staffEntity.Staff_fName,
                staffEntity.Staff_lName,
                staffEntity.Staff_PhoneNum,
                staffEntity.Staff_Grade,
                staffEntity.Staff_Active
                );
        }

    }

    //this class is used as a return value. Records are like mini-classes with simplified declaration 
    public record StaffDto(int Staff_ID, 
        string Staff_fName,
        string Staff_lName,
        string Staff_PhoneNum,
        int Staff_Grade,
        Boolean Staff_Active
        ); 

}
