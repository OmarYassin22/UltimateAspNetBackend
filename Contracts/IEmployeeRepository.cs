using Entities.models;
using Entities.ReauestFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParamtere paramtere, bool trackChanges);
        Task<Employee?> GetEmployeeAsync(Guid companyId,Guid employeeId,bool trackChanges);
        Task<bool> GetCompanyAsync(Guid companyId);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
;