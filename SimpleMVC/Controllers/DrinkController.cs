using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;
        private string SessionToken => HttpContext.Session.GetString("JWToken");

        public DrinkController(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllDrinks()
        {
            return Json(new { data = await _drinkRepository.GetAllAsync(SessionToken) });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return View(new Drink());

            Drink drink = await _drinkRepository.GetAsync((int)id, SessionToken);
            if(drink is not null)
            {
                return View(drink);
            }

            return NotFound();
        }

        public async Task<IActionResult> Upsert(Drink drink)
        {
            if (ModelState.IsValid)
            {
                if (drink.Id == 0) await _drinkRepository.CreateAsync(drink, SessionToken);
                else await _drinkRepository.UpdateAsync(drink, SessionToken);

                return RedirectToAction(nameof(Index));
            }
            else return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            if (await _drinkRepository.DeleteAsync(id, SessionToken)) // da vedere se scoppia
            {
                return Json(new { success = true, message = "Contact Deleted" });
            }
            return Json(new { success = false, message = "Coudn't delete the Contact" });
        }
    }
}
