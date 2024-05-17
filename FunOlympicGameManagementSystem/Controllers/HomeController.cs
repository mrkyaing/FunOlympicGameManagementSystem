using FunOlympicGameManagementSystem.Models;
using FunOlympicGameManagementSystem.Models.DAO;
using FunOlympicGameManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FunOlympicGameManagementSystem.Controllers {
    public class HomeController : Controller {
        
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appContext;

        public HomeController(ILogger<HomeController> logger,AppDbContext appContext) {
            _logger = logger;
            this._appContext = appContext;
        }

        public IActionResult Index() {
            //var loggedInUser = HttpContext.User;
            //TempData["loggedInUserName"] = loggedInUser.Identity.Name; // This is our username we set earlier in the claims. 
            return View();
        }

        public IActionResult About() {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ContactEnquiry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactEnquiry(ContactEnquiryModel contact)
        {
            try
            {
                ContactEnquiryEntity contactEnquiry = new ContactEnquiryEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Message = contact.Message
                };
                _appContext.ContactEnquiries.Add(contactEnquiry);
                _appContext.SaveChanges();
                ViewBag.Msg = "Send successfully the contact information to the system.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occur when send the contact information to the system." +e.Message;
            }
            return View();
        }
        public IActionResult ContactEnquiryList()
        {
            var contacts = _appContext.ContactEnquiries.Select(contact => new ContactEnquiryModel {
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Message = contact.Message
            }).ToList();
            return View(contacts);
        }
    }
}