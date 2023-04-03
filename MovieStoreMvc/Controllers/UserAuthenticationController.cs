using Microsoft.AspNetCore.Mvc;
using MoviesMvc.Models.DTO;
using MoviesMvc.Repositories.Abstract;

namespace MoviesMvc.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authService;

        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> Register()
        {
            var model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                Username = "admin",
                Name = "Emina",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin",
            };
            var result = await _authService.RegisterAsync(model);
            return Ok(result.Message);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _authService.LoginAsync(model);
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Could not logged in.";
                return RedirectToAction(nameof(Login));
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
