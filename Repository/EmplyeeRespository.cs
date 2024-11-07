using Contracts;
using Entities;
using Entities.models;
using Entities.ReauestFeature;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmplyeeRespository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
    {
        //public EmplyeeRespository() : base(context) { }


        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParamtere paramtere, bool trackChanges)

        {
            var employees = await FindByConditions(e => e.CompanyId.Equals(companyId) && e.Age >= paramtere.MinAge && e.Age <= paramtere.MaxAge, trackChanges)
                .FilterEmployee(paramtere.MinAge, paramtere.MaxAge)
                .Search(paramtere.SearchTerm).
                Sort(paramtere.Orderby)
               .ToListAsync();

            var res = PagedList<Employee>.ToPagedList(employees,
            paramtere.PageNumber,
            paramtere.PageSize);
            return res;

        }
        public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        => await FindByConditions(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefaultAsync();

        public async Task<bool> GetCompanyAsync(Guid companyId)
       => await context.Companies.AnyAsync(c => c.Id.Equals(companyId));

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }
        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

    }
}
