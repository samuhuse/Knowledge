using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleApi.Model;
using SimpleApi.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        public static List<string> _names;

        [HttpGet]
        public IActionResult GetAllContact()
        {
            return Ok(GenerateContact(_names));
        }

        [HttpGet("ByNominative")]
        public IActionResult GetContactsByNominative(string nominative)
        {
            return Ok(GenerateContact(_names).Where(c => c.Name.Equals(nominative) || c.Surname.Equals(nominative)));
        }

        [HttpGet("ByName")]
        public IActionResult GetContactsByName(string nominative)
        {
            return Ok(GenerateContact(_names).Where(c => c.Name.Equals(nominative)));
        }

        [HttpGet("BySurname")]
        public IActionResult GetContactsBySurname(string nominative)
        {
            return Ok(GenerateContact(_names).Where(c => c.Surname.Equals(nominative)));
        }

        [HttpPost]
        public IActionResult AddName(string name)
        {
            if (_names.Contains(name))
            {
                return BadRequest();
            }
            else
            {
                _names.Add(name);
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteName(string name)
        {
            if (_names.Contains(name))
            {
                _names.Remove(name);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IActionResult UpdateName(string name, string newName)
        {
            if (_names.Contains(name))
            {
                _names.Remove(name);
                _names.Add(newName);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public ContactController()
        {
            _names =
               new List<string>
               {
                "SMITH"
                ,"BROWN"
                ,"WILSON"
                ,"THOMSON"
                ,"ROBERTSON"
                ,"CAMPBELL"
                ,"STEWART"
                ,"ANDERSON"
                ,"MACDONALD"
                ,"SCOTT"
                ,"REID"
                ,"MURRAY"
                ,"TAYLOR"
                ,"CLARK"
                ,"MITCHELL"
                ,"ROSS"
                ,"WALKER"
                ,"PATERSON"
                ,"YOUNG"
               };

        }

        private static IEnumerable<Contact> GenerateContact(List<string> names)
        {
            Random random = new Random();
            string GetRandomName()
            {
                return names.ElementAt(random.Next(names.Count));
            }

            List<Contact> bufferCollision = new List<Contact>();

            int c = names.Count().GetPermutation();
            while(c > 1)
            {
                Contact contact = null;
                while (contact is null)
                {
                    contact = ContactFactory.Create(GetRandomName(), GetRandomName());
                    if (bufferCollision.Contains(contact)) contact = null;
                    else { bufferCollision.Add(contact);}
                }

                c--;
                yield return contact;
            }
        }
    }
}
