namespace Entities.ReauestFeature
{
    public class EmployeeParamtere : ReauestParamters
    {
        public EmployeeParamtere()
        {
            Orderby = "name";
        }
        public uint MinAge { get; set; } = 18;
        public uint MaxAge { get; set; } = 60;
        public bool IsValidAgeRange => MaxAge > MinAge;
        public string SearchTerm { get; set; }
    }
}
