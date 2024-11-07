using Api_New_Feature.ActionFitler;
using Api_New_Feature.DataTransfareObject;
using Api_New_Feature.ModelBinders;
using Azure;
using Contracts;
using Entities.models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.InteropServices;

namespace Api_New_Feature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICompanyRepository _repository;

        public CompanyController(ILoggerManager logger, ICompanyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {


            var companies = await _repository.GetAllCompaniesAsync(false);
            if (companies == null)
            {
                _logger.LogError("Companies object sent from repository is null.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo("Returned all companies from database.");

                TypeAdapterConfig<Company, CompanyDTO>.NewConfig().Map(d => d.FullAddress, s => s.Address + " " + s.Country).Map(d => d.employees, s => s.Employees);


                var result = companies.Adapt<IEnumerable<CompanyDTO>>();
                return Ok(JsonConvert.SerializeObject(result));
            }
        }
        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetComapny(Guid id)
        {

            var company = await _repository.GetCompanyAsync(id, false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            TypeAdapterConfig<Company, CompanyDTO>.NewConfig().Map(s => s.FullAddress, d => d.Address + " " + d.Country);
            var result = company.Adapt<CompanyDTO>();
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreateionDTO company)
        {
            // not need for validation check as use ActionFilter for validation
            //if (company is null)
            //{
            //    _logger.LogError("CompanyForCreationDto object sent from client is null.");
            //    return BadRequest("CompanyForCreationDto object is null");
            //}
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("invalied Model");
            //    return UnprocessableEntity(ModelState);
            //}
            TypeAdapterConfig<CompanyForCreateionDTO, Company>.NewConfig().Map(d => d.Employees, s => s.employees);
            var result = company.Adapt<Company>();
            _repository.createCompany(result);
            await _repository.saveAsync();

            return Ok(result);

        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreateionDTO> companies)
        {
            if (companies is null)
            {
                _logger.LogError("Companies is required for createion");
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("invalied Model");
                return UnprocessableEntity(ModelState);
            }
            var companyEntites = companies.Adapt<IEnumerable<Company>>();
            foreach (var company in companyEntites)
            {
                _repository.createCompany(company);


            }
            await _repository.saveAsync();
            TypeAdapterConfig<Company, CompanyDTO>.NewConfig().Map(d => d.FullAddress, s => s.Address + " " + s.Country);

            var result = companyEntites.Adapt<IEnumerable<CompanyDTO>>();
            return Ok(result);

        }

        [HttpGet("collection/({ids})")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {

            if (ids is null)
            {
                _logger.LogError("Ids parameter is null");
                return BadRequest("Ids parameter is null");
            }
            var companyEntities = await _repository.GetByIdsAsync(ids, false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound("Some ids are not valid in a collection");
            }
            var result = companyEntities.Adapt<IEnumerable<CompanyDTO>>();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyWithEmployeers(Guid id)
        {
            var company = await _repository.GetCompanyAsync(id, false);
            if (company is null)
            {
                _logger.LogError($"Company with id: {id}, hasn't been found in db.");
                return NotFound();

            }
            _repository.DeleteCompanyWithEmployee(company);
            await _repository.saveAsync();
            return NoContent();

        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDTO company)
        {
            //if (company is null)
            //{
            //    _logger.LogError("CompanyForUpdateDTO object sent from client is null.");
            //    return BadRequest("CompanyForUpdateDTO object is null");
            //}
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("invalied Model");
            //    return UnprocessableEntity(ModelState);
            //}
            var entity = await _repository.GetCompanyAsync(id, true);
            if (entity is null)
            {
                _logger.LogError($"company with id: {id} is not exist in DB");
                return BadRequest();
            }

            company.Adapt(entity);
            //_mapper.Map(company, entity);
            await _repository.saveAsync();
            return Ok(entity);


        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForUpdateDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("JsonPatchDocument object sent from client is null.");
                return BadRequest("JsonPatchDocument object is null");

            }

            var entity = await _repository.GetCompanyAsync(id, true);
            if (entity is null)
            {
                _logger.LogError($"company with id: {id} is not exist in DB");
                return BadRequest();
            }
            var companyToPatch = entity.Adapt<CompanyForUpdateDTO>();
            patchDoc.ApplyTo(companyToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                _logger.LogError("invalied Model");
                return UnprocessableEntity(ModelState);
            }
            companyToPatch.Adapt(entity);
            return Ok(entity);

        }

    }
}
