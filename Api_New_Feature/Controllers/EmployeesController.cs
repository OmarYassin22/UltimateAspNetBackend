using Api_New_Feature.DataTransfareObject;
using Azure;
using Contracts;
using Entities.models;
using Entities.ReauestFeature;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;

namespace Api_New_Feature.Controllers
{
    [Route("api/company/{companyId}/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILoggerManager _logger;

        public EmployeesController(IEmployeeRepository repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyID, [FromQuery] EmployeeParamtere paramtere)
        {
            var company = await _repository.GetCompanyAsync(companyID);
            if(!paramtere.IsValidAgeRange)
                return BadRequest("Max age can't be less than min age");
            if (!company)
            {
                _logger.LogError($"Company with id: {companyID}, hasn't been found in db.");
                return NotFound();
            }
            var employees =await  _repository.GetEmployeesAsync(companyID, paramtere, false);
            if (employees is null)
            {

                _logger.LogError($"Company with id: {companyID}, hasn't been found in db.");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(employees.MetaData));
            TypeAdapterConfig<Company, CompanyDTO>.NewConfig().Map(s => s.employees, d => d.Employees);
            var result = employees.Adapt<IEnumerable<EmployeeDTO>>();
            return Ok(result);


        }
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var company = await _repository.GetCompanyAsync(companyId);
            if (!company)
            {
                _logger.LogError($"Company with id: {companyId}, hasn't been found in db.");
                return NotFound();
            }

            var employee = await _repository.GetEmployeeAsync(companyId, employeeId, false);
            if (employee is null)
            {
                _logger.LogError($"Company with id :{companyId}, doesn't have employee with id: {employeeId}");
                return NotFound();


            }
            var result = employee.Adapt<EmployeeDTO>();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDTO employee)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("invlid model state");
                return UnprocessableEntity(ModelState);
            }
            if (employee == null)
            {

                _logger.LogError("EmployeeForCreationDTO object sent from client is null.");
                return BadRequest(employee);
            }
            var company = await _repository.GetCompanyAsync(companyId);
            if (!company)
            {
                _logger.LogError($"Company with id: {companyId}, hasn't been found in db.");
                return NotFound();

            }

            var empEntity = employee.Adapt<Employee>();
            _repository.CreateEmployeeForCompany(companyId, empEntity);
            await _repository.saveAsync();
            return Ok(empEntity);

        }
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var company = await _repository.GetCompanyAsync(companyId);
            if (!company)
            {
                _logger.LogError($"Company with id: {companyId}, hasn't been found in db.");
                return NotFound();
            }
            var employee = await _repository.GetEmployeeAsync(companyId, employeeId, false);
            if (employee is null)
            {
                _logger.LogError($"Company with id :{companyId}, doesn't have employee with id: {employeeId}");
                return NotFound();
            }
            _repository.DeleteEmployee(employee);
            await _repository.saveAsync();
            return NoContent();
        }
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDTO employee)
        {
            if (employee is null)
            {
                _logger.LogError("EmployeeForUpdateDTO object sent from client is null.");
                return BadRequest("EmployeeForUpdateDTO object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("invalied Model");
                return UnprocessableEntity(ModelState);
            }
            var company = await _repository.GetCompanyAsync(companyId);
            if (!company)
            {
                _logger.LogError($"Company with id: {companyId}, hasn't been found in db.");
                return NotFound();
            }
            var entity = await _repository.GetEmployeeAsync(companyId, employeeId, true);
            if (entity is null)
            {
                _logger.LogError($"Company with id :{companyId}, doesn't have employee with id: {employeeId}");
                return NotFound();
            }
            employee.Adapt(entity);
            _repository.Update(entity);
            await _repository.saveAsync();

            return NoContent();

        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId, [FromBody] JsonPatchDocument<EmployeeForUpdateDTO> document)
        {

            if (document is null)
            {
                _logger.LogError("JsonPatchDocument object sent from client is null.");
                return BadRequest("JsonPatchDocument object is null");

            }


            var company = await _repository.GetCompanyAsync(companyId);
            if (!company)
            {
                _logger.LogError($"Company with id: {companyId}, hasn't been found in db.");
                return NotFound();
            }
            var entity = await _repository.GetEmployeeAsync(companyId, employeeId, true);
            if (entity is null)
            {

                _logger.LogError($"Company with id :{companyId}, doesn't have employee with id: {employeeId}");
                return NotFound();

            }

            // get DTO with same data from DB
            var employeeToPatch = entity.Adapt<EmployeeForUpdateDTO>();
            // apply patch for this DTO

            document.ApplyTo(employeeToPatch, ModelState);

            TryValidateModel(employeeToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("invalied Model");
                return UnprocessableEntity(ModelState);
            }
            // apply DTO to entity FROM DB
            employeeToPatch.Adapt(entity);


            await _repository.saveAsync();
            return NoContent();

        }
    }
}
