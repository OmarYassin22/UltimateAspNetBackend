using Contracts;
using Entities;
using Entities.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRespository : RepositoryBase<Company>, ICompanyRepository
    {
        private readonly RepositoryContext context;

        public CompanyRespository(RepositoryContext context) : base(context)
        {
            this.context = context;
        }

        public void createCompany(Company company)
        => Create(company);

        public void DeleteCompanyWithEmployee(Company company)
       => Delete(company);

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        =>await context.Companies.ToListAsync();

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        => await FindByConditions(c => ids.Contains(c.Id), trackChanges).ToListAsync();

        public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges)
        => await FindByConditions(c => c.Id == companyId, trackChanges).Include(c=>c.Employees).FirstOrDefaultAsync();


    }
}
