using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleApi.Model;
using SimpleApi.Model.Dtos;
using SimpleApi.Model.Dtos.Read;
using SimpleApi.Model.Dtos.Update;
using SimpleApi.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CompanyReadDto>))]
        public IActionResult GetAllCompanies()
        {
            List<CompanyReadDto> contactDtoList = new List<CompanyReadDto>();
            foreach (Company company in _companyRepository.GetAll().ToList())
            {
                contactDtoList.Add(_mapper.Map<CompanyReadDto>(company));
            }

            return Ok(contactDtoList);
        }

        [HttpGet("id:int", Name ="GetCompany")]
        [ProducesResponseType(200, Type = typeof(CompanyReadDto))]
        [ProducesResponseType(404)]
        public IActionResult GetCompany(int id)
        {
            Company company = _companyRepository.SingleOrDefault(c => c.Id == id);

            if (company is not null) return Ok(_mapper.Map<CompanyReadDto>(company));
            else return NotFound();
        }

        [HttpPut]
        public IActionResult AddCompany([FromBody] CompanyCreateDto companyDto)
        {
            if(companyDto is null) { return BadRequest(ModelState); }
            if(_companyRepository.Exists(c => c.Name == companyDto.Name))
            {
                ModelState.AddModelError("", "Company already exists in the database");
                return StatusCode(500, ModelState);
            }

            Company company = _mapper.Map<Company>(companyDto);
            _companyRepository.Add(company);
            if (_companyRepository.Save() > 0)
            {
                return CreatedAtRoute("GetCompany", company.Id, company);
            }
            else
            {
                ModelState.AddModelError("", "Something went erong saving the company");
                return StatusCode(500, ModelState);
            }
        }

        [HttpPatch("id:int")]
        public IActionResult UpdateCompany(int id, [FromBody] CompanyUpdateDto companyDto) 
        {
            if (companyDto is null || id != companyDto?.Id) { return BadRequest(); }
            if(!_companyRepository.Exists(c => c.Id == id))
            {
                ModelState.AddModelError("", "Company doesn't exist in the database");
                return StatusCode(500, ModelState);
            }

            Company company = _mapper.Map<Company>(companyDto);
            _companyRepository.Update(company);
            if (_companyRepository.Save() > 0)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating the company");
                return StatusCode(500, ModelState);
            }
        }

        [HttpDelete("id:int")]
        public IActionResult DeleteCompany(int id)
        {
            Company company = _companyRepository.SingleOrDefault(c => c.Id == id);
            
            if(company is null) 
            {
                ModelState.AddModelError("", "Company doesn't exists in the database");
                return StatusCode(500, ModelState);
            }

            _companyRepository.Remove(company);
            if(_companyRepository.Save() > 0)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "something wen wrong deleting the company");
                return StatusCode(500, ModelState);
            }
        }
    }
}
