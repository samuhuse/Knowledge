using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleApi.Model;
using SimpleApi.Model.Dtos;
using SimpleApi.Model.Dtos.Create;
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
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactsController(IContactRepository ContactRepository, IMapper mapper)
        {
            _contactRepository = ContactRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all Contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ContactReadDto>))]
        public IActionResult GetAllContacts()
        {
            List<ContactReadDto> contactDtoList = new List<ContactReadDto>();
            foreach (Contact contact in _contactRepository.GetAll())
            {
                contactDtoList.Add(_mapper.Map<ContactReadDto>(contact));
            }

            return Ok(contactDtoList);
        }

        /// <summary>
        /// Gets all company contacts
        /// </summary>
        /// <param name="companyId">Id of the Company</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ContactReadDto>))]
        [Route("ByCompany")]
        public IActionResult GetCompanyContacts(int companyId)
        {
            List<ContactReadDto> contactDtoList = new List<ContactReadDto>();
            foreach (Contact contact in _contactRepository.GetByCompanyId(companyId))
            {
                contactDtoList.Add(_mapper.Map<ContactReadDto>(contact));
            }

            return Ok(contactDtoList);
        }

        /// <summary>
        /// Get Contact by id
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <returns></returns>
        [HttpGet("{id:int}",Name = "GetContact")]
        [ProducesResponseType(200, Type = typeof(ContactReadDto))]
        [ProducesResponseType(404)]
        public IActionResult GetContact(int id)
        {
            Contact contact = _contactRepository.SingleOrDefault(c => c.Id == id);

            if (contact is not null) return Ok(_mapper.Map<ContactReadDto>(contact));
            else return NotFound();
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        /// <param name="contactDto">Content of the new Contact</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ContactCreateDto))]
        [ProducesResponseType(500)]
        public IActionResult AddContact([FromBody] ContactCreateDto contactDto)
        {
            if (contactDto is null) return BadRequest(ModelState);

            if(_contactRepository.Exists(c => c.Name == contactDto.Name && c.Surname == contactDto.Surname))
            {
                ModelState.AddModelError("", "contact already in the database");
                return StatusCode(500, ModelState);
            }

            Contact contact = _mapper.Map<Contact>(contactDto);

            _contactRepository.Add(_mapper.Map<Contact>(contactDto));

            if (_contactRepository.Save() > 0)
            {
                return CreatedAtRoute("GetContact",new { contact.Id }, contact);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong savign the contact");
                return StatusCode(500, ModelState);
            }            
        }

        /// <summary>
        /// Update an existing Contact
        /// </summary>
        /// <param name="id">Id of the Contact to update</param>
        /// <param name="contactDto">New content of the Contact to update</param>
        /// <returns></returns>
        [HttpPatch(Name = "UpdateContact")]        
        [ProducesResponseType(204)]        
        [ProducesResponseType(500)]
        public IActionResult UpdateContact([FromBody] ContactUpdateDto contactDto)
        {
            if (contactDto.Id == 0) return BadRequest(ModelState);
            if (!_contactRepository.Exists(c => c.Id == contactDto.Id))
            {
                ModelState.AddModelError("", "contact doesn't exist in the database");
                return StatusCode(500, ModelState);
            }

            Contact contact = _mapper.Map<Contact>(contactDto);

            _contactRepository.Update(_mapper.Map<Contact>(contactDto));
            if (_contactRepository.Save() > 0)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating the contact");
                return StatusCode(500, ModelState);
            }
        }

        /// <summary>
        /// Delete a Contact
        /// </summary>
        /// <param name="id">Id of the Contact to delete</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteContact")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult DeleteContact(int id)
        {
            Contact contactoRemove = _contactRepository.SingleOrDefault(c => c.Id == id);
            if (contactoRemove is null)
            {
                ModelState.AddModelError("", "contact doesn't exist in the database");
                return NotFound();
            }

            _contactRepository.Remove(contactoRemove);
            if (_contactRepository.Save() > 0)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong deleting the contact");
                return StatusCode(500, ModelState);
            }
        }
    }
}
