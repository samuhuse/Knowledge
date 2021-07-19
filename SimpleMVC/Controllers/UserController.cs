using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleMvcConsumer.Model.Authorization;
using SimpleMvcConsumer.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository _userRepository)
        {
            this._userRepository = _userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            Credential credential = new Credential();
            return View(credential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(Credential credential)
        {
            User user = await _userRepository.LogInAsync(credential);

            if (user.Token is null) return View();

            HttpContext.Session.SetString("JWToken", user.Token);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {
            Credential credential = new Credential();
            return View(credential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Credential credential)
        {
            User user = await _userRepository.RegisterAsync(credential);

            if (user.Token is null) return View();

            HttpContext.Session.SetString("JWToken", user.Token);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index","Home");
        }
    }
}
