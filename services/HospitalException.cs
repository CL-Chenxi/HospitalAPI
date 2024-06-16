namespace HospitalAPI.services
{
    public class HospitalException : Exception
    {
        public HospitalException(string message) : base(message) { }
        public HospitalException() { }
    }
}
