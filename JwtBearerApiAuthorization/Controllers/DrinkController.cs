using Microsoft.AspNetCore.Mvc;

using JwtBearerApiAuthorization.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using JwtBearerApiAuthorization.Model;

namespace JwtBearerApiAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkController(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_drinkRepository.GetAll());
        }

        [Authorize]
        [HttpGet("{id:int}", Name ="GetDrink")]    
        public IActionResult GetById(int id)
        {
            Drink drink = _drinkRepository.Get(id);
            if (drink is null) return NotFound();
            return Ok(drink);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("ByIdAdmin/{id:int}")]
        public IActionResult GetByIdAdmin(int id)
        {
            Drink drink = _drinkRepository.Get(id);
            if (drink is null) return NotFound();
            return Ok(drink);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Drink drink = _drinkRepository.Get(id);

            if (drink is null) return NotFound();

            _drinkRepository.Remove(drink);
            if (_drinkRepository.Save() > 0) return NoContent();

            ModelState.AddModelError("", "Something went wrong deleting the drink");
            return StatusCode(500, ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Drink drink)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_drinkRepository.Exists(c => c.Name == drink.Name))
            {
                ModelState.AddModelError("", "drink already exist in the database");
                return StatusCode(500, ModelState);
            }

            _drinkRepository.Add(drink);
            if (_drinkRepository.Save() > 0)
            {
                return CreatedAtRoute("GetDrink", new { drink.Id }, drink);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating the drink");
                return StatusCode(500, ModelState);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch]
        public IActionResult Update(Drink drink)
        {
            if (drink.Id == 0) return BadRequest(ModelState);
            if (!_drinkRepository.Exists(c => c.Id == drink.Id))
            {
                ModelState.AddModelError("", "drink doesn't exist in the database");
                return StatusCode(500, ModelState);
            }

            _drinkRepository.Update(drink);
            if (_drinkRepository.Save() > 0)
            {
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating the drink");
                return StatusCode(500, ModelState);
            }
        }
    }
}
