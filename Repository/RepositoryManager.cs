using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManger
    {
        private readonly RepositoryContext context;

        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        public RepositoryManager(RepositoryContext context)
        {
            this.context = context;
        }

        public ICompanyRepository Company
        {
            get
            {

                if (_companyRepository is null)
                {
                    _companyRepository = new CompanyRespository(context);
                }
                return _companyRepository;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {

                if (_employeeRepository is null)
                    _employeeRepository = new EmplyeeRespository(context);
                return _employeeRepository;
            }
        }

        public void Save() => context.SaveChanges();
    }

}
