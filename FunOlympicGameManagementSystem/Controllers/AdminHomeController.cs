using FunOlympicGameManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    [Authorize]
    public class AdminHomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult SportTypes() {
            var sportTypes=new List<SportTypeEntity> {
                new SportTypeEntity{Code="Sp1",Name= "Archery" },
                new SportTypeEntity { Code = "Sp2", Name = "Basketball" } ,
                new SportTypeEntity{Code="Sp3",Name= "Boxing" },
                new SportTypeEntity { Code = "Sp4", Name = "Cycling" },
                new SportTypeEntity{Code="Sp5",Name= "Fencing" },
                new SportTypeEntity { Code = "Sp6", Name = "Shooting" },
                new SportTypeEntity{Code="Sp7",Name= "Surfing" },
                new SportTypeEntity { Code = "Sp8", Name = "Taekwondo" },
               new SportTypeEntity{Code="Sp9",Name= "Badminton" },
            };
            return View(sportTypes);
        }

         public IActionResult BrocastTypes() {
            var sportTypes = new List<BroadCastTypeEntity> {
                new BroadCastTypeEntity{Code="BCT1",Name= "BBC" },
                new BroadCastTypeEntity { Code = "BCT2", Name = "NBC" } ,
                new BroadCastTypeEntity{Code="BCT13",Name= "Olympic" },
                new BroadCastTypeEntity { Code = "BCT14", Name = "SBS" },
                new BroadCastTypeEntity{Code="BCT15",Name= "SKY New Zealand" },
                new BroadCastTypeEntity { Code = "BCT16", Name = "NineNetwork Austrialia" },

            };
            return View(sportTypes);
        }
    }
}
