using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using OneStop.Models;
using OneStop.ViewModels;
using System.IO;

namespace OneStop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var thisTourist = _db.Tourists.FirstOrDefault(item => item.UserName == User.Identity.Name);
                return View(thisTourist);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
               ViewBag.Error = "Please re-enter your credentials";
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
   
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "No match for username and password";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tourist tourist, IFormFile avatar)
        {
            tourist.UserName = User.Identity.Name;
            byte[] profilePic = ConvertToBytes(avatar);
            tourist.Pic = profilePic;
            _db.Tourists.Add(tourist);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var editedTourist = _db.Tourists.FirstOrDefault(tourists => tourists.TouristId == id);
            return View(editedTourist);
        }

        [HttpPost]
        public IActionResult Edit(Tourist tourist, IFormFile avatar)
        {
            var editedTourist = _db.Tourists.FirstOrDefault(tourists => tourists.TouristId == tourist.TouristId);
            _db.Tourists.Attach(editedTourist);
            byte[] profilePic = ConvertToBytes(avatar);
            editedTourist.Name = tourist.Name;
            editedTourist.Pic = profilePic;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        private byte[] ConvertToBytes(IFormFile image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }
    }
}
