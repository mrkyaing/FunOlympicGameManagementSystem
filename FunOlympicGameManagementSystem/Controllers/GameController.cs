using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    public class GameController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
