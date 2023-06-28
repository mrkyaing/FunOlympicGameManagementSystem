using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    [Authorize]
    public class AdminHomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
