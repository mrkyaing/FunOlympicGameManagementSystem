using FunOlympicGameManagementSystem.Models;
using FunOlympicGameManagementSystem.Models.DAO;
using FunOlympicGameManagementSystem.Models.ViewModels;
using FunOlympicGameManagementSystem.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login(UserViewModel userViewModel) {
          
            bool IsActivateEmail = _appDbContext.Users.Any(x => x.Email == userViewModel.Email && x.IsEmailVerification==false);
            if (IsActivateEmail) {
                ViewBag.Msg = "You can't login because you are not activated user.Please activate first.";
                ViewBag.ActivateURL = "<a href=\"/account/UserVerificationWithOtp\">Click here to activate your account</a>.";
                return View();
            }
            var _passWord = EncryptPassword.TextToEncrypt(userViewModel.Password);
            var user = _appDbContext.Users.Where(x => x.Email == userViewModel.Email && x.IsEmailVerification &&
            x.Password == _passWord).SingleOrDefault();
            if (user is not null) {
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties{ExpiresUtc = DateTime.Now.AddMinutes(10)};
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),authProperties);
                if (user.Role.Equals(Roles.Admin.ToString()))
                    return RedirectToAction("Index", "AdminHome");
                else 
                    return RedirectToAction("Index", "Home");
            }
            else {
                ViewBag.Msg= "Invalid Information... Please try again!";
            }
            return View();
        }
        [Authorize]
        public async Task< IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
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
                        Role= Roles.User.ToString(),
                    };
                    _appDbContext.Users.Add(userEntity);
                    _appDbContext.SaveChanges();
                    string otp = OtpHelper.GetRandom6Digit();
                    OtpHelper.SendOtpToUserEmail(userViewModel.Email, otp);
                    OTPEntity oTPEntity=OtpHelper.OtpCreateWithEmail(userViewModel.Email, otp);
                    _appDbContext.OTPs.Add(oTPEntity);
                    _appDbContext.SaveChanges();
                    ViewBag.Msg = "We send OTP to this email " + userViewModel.Email;
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
