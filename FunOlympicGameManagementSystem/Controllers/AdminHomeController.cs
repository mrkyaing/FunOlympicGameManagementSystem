using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    public class AdminHomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
