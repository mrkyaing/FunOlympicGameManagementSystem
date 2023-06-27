using FunOlympicGameManagementSystem.Models;
using FunOlympicGameManagementSystem.Models.DAO;
using FunOlympicGameManagementSystem.Models.ViewModels;
using FunOlympicGameManagementSystem.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FunOlympicGameManagementSystem.Controllers {
    public class AccountController : Controller {

        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _appDbContext;

        public AccountController(ILogger<AccountController> logger,AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }
        public IActionResult Login() {
            _logger.LogInformation("login page loading");
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserViewModel userViewModel) {
          
            bool IsActivateEmail = _appDbContext.Users.Any(x => x.Email == userViewModel.Email && x.IsEmailVerification==false);
            if (IsActivateEmail) {
                ViewBag.Msg = "You can't login because you are not activated user.Please activate first.";
                ViewBag.ActivateURL = "<a href=\"/account/UserVerificationWithOtp\">Click here to activate your account</a>.";
                return View();
            }
            var _passWord = EncryptPassword.TextToEncrypt(userViewModel.Password);
            bool Isvalid = _appDbContext.Users.Any(x => x.Email == userViewModel.Email && x.IsEmailVerification &&
            x.Password == _passWord);
            if (Isvalid) {
                //int timeout = LgnUsr.Rememberme ? 60 : 5; // Timeout in minutes, 60 = 1 hour.    
                //var ticket = new FormsAuthenticationTicket(LgnUsr.EmailId, false, timeout);
                //string encrypted = FormsAuthentication.Encrypt(ticket);
                //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                //cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                //cookie.HttpOnly = true;
                //Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "AdminHome");
            }
            else {
                ViewBag.Msg= "Invalid Information... Please try again!";
            }
            return View();
        }
        public IActionResult Register() {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserViewModel userViewModel) {
            if(ModelState.IsValid) {
                var isEmailAlreadyExists = this.IsEmailExists(userViewModel.Email);
                if(isEmailAlreadyExists) {
                    ModelState.AddModelError("EmailExists",$"{userViewModel.Email} is already exist in system");
                return View();
                }
                try {
                    UserEntity userEntity = new UserEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = userViewModel.UserName,
                        Email = userViewModel.Email,
                        Password = EncryptPassword.TextToEncrypt(userViewModel.Password),
                        DOB = userViewModel.DOB,
                        Address = userViewModel.Address,
                        Country = userViewModel.Country,
                        Gender = userViewModel.Gender,
                    };
                    _appDbContext.Users.Add(userEntity);
                    _appDbContext.SaveChanges();
                    string otp = OtpHelper.GetRandom6Digit();
                    OtpHelper.SendOtpToUserEmail(userViewModel.Email, otp);
                    OTPEntity oTPEntity=OtpHelper.OtpCreateWithEmail(userViewModel.Email, otp);
                    _appDbContext.OTPs.Add(oTPEntity);
                    _appDbContext.SaveChanges();
                    ViewBag.Msg = "Register successfully and we send OTP to this email :" + userViewModel.Email;
                    ViewBag.ActivateURL= "<a href=\"/account/UserVerificationWithOtp\">Click here to activate your account</a>.";
                     
                }
                catch (Exception e) {
                    ViewBag.Msg = "Register failed" +e.Message;
                }
            }
            return View();
        }
        public IActionResult UserVerificationWithOtp()=>View();
        [HttpPost]
        public IActionResult UserVerificationWithOtp(string email,string otp) {
            var correctOtp=_appDbContext.OTPs.Any(x=>x.IsActive && x.OTP == otp && x.EmailId==email);
            if (correctOtp) {
                UserEntity u = _appDbContext.Users.Where(x => x.Email == email).SingleOrDefault();
                u.IsEmailVerification=true;
                _appDbContext.Entry(u).State = EntityState.Modified;//Updating the existing recrod in db set 
                _appDbContext.SaveChanges();//Updating  the record to the database
                ViewBag.Msg = "Account verification is completed.";
                return View();
            }
            return View();
        }
        private bool IsEmailExists(string eMail) {
            var IsCheck = _appDbContext.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }
    }
}
