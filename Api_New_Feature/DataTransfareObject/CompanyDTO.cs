namespace Api_New_Feature.DataTransfareObject
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public IEnumerable<EmployeeDTO> employees { get; set; }
    }
}
