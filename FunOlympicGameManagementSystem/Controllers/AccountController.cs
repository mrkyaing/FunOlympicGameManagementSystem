using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger) {
            _logger = logger;
        }
        public IActionResult Login() {
            _logger.LogInformation("login page loading");
            return View();
        }

        public IActionResult Register() {
            return View();
        }
    }
}
