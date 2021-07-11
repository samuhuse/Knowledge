
using Microsoft.AspNetCore.Mvc;

using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository;
using SimpleMvcConsumer.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return View(new Contact());
            }

            Contact contact = await _contactRepository.GetAsync((int)id);
            if (contact is not null) // da vedere se scoppia
            {
                return View(contact);
            }

            return NotFound();
        }

        public async Task<IActionResult> Upsert(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.Id == 0)
                {
                    await _contactRepository.CreateAsync(contact);
                }
                else
                {
                    bool al = await _contactRepository.UpdateAsync(contact);
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
             
            if (await _contactRepository.DeleteAsync(id)) // da vedere se scoppia
            {
                return Json(new { success = true, message = "Contact Deleted" });
            }
            return Json(new { success = false, message = "Coudn't delete the Contact" });
        }

        public async Task<IActionResult> GetAllContact()
        {
            return Json(new { data = await _contactRepository.GetAllAsync()});            
        }


    }
}
