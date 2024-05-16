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
                if (user.Role.Equals(Roles.Admin.ToString())|| user.Role.Equals(Roles.SystemAdmin.ToString()))
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
            var loggedInUser = HttpContext.User;
            var userEmailId = loggedInUser.Identity.Name; // This is our username we set earlier in the claims. 
            var otpsLoginedUser=_appDbContext.OTPs.Where(x=>x.EmailId.Equals(userEmailId));
            _appDbContext.OTPs.RemoveRange(otpsLoginedUser);
           await _appDbContext.SaveChangesAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View(CountryEntity.GetAllCities());
        }

        [HttpPost]
        public IActionResult Register(UserViewModel userViewModel) {
            if(ModelState.IsValid) {
                var isEmailAlreadyExists = this.IsEmailExists(userViewModel.Email);
                if(isEmailAlreadyExists) {
                    ModelState.AddModelError("EmailExists",$"{userViewModel.Email} is already exist in system.");
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
                    OtpHelper.SendOtpToUserEmail(userViewModel.Email, otp,"Account creation", "account creation process");
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
            return View(CountryEntity.GetAllCities());
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
                ViewBag.Msg = "Account verification process is completed.";
                return View();
            }
            return View();
        }
        private bool IsEmailExists(string email) {
            var IsCheck = _appDbContext.Users.Where(x => x.Email == email&&x.IsActive).FirstOrDefault();
            return IsCheck != null;
        }
        public ActionResult ForgetPassword()=> View();

        [HttpPost]
        public ActionResult ForgetPassword(string EmailId) {
            var IsExists = IsEmailExists(EmailId);
            if (!IsExists) {
                ModelState.AddModelError("EmailNotExists", "This email is not exists.");
                return View();
            }
            var user = _appDbContext.Users.Where(x => x.Email == EmailId).FirstOrDefault();
            string otp = OtpHelper.GetRandom6Digit();
            OtpHelper.SendOtpToUserEmail(EmailId, otp,"Account verification", "Account verification process");
            OTPEntity oTPEntity = OtpHelper.OtpCreateWithEmail(EmailId, otp);
            _appDbContext.OTPs.Add(oTPEntity);
            _appDbContext.SaveChanges();
            ViewBag.Msg = "We send OTP to this email " + EmailId;
            ViewBag.ActivateURL = "<a href=\"/account/ChangePassword\">Click here to reset your forget password.</a>.";
            TempData["resetEmailId"] = EmailId;
            return View();
        }

        public ActionResult ChangePassword() => View();
        
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel) {
            var correctOtp = _appDbContext.OTPs.Any(x => x.IsActive && x.OTP == changePasswordViewModel.OTP && x.EmailId == changePasswordViewModel.Email);
            if (correctOtp) {
                UserEntity u = _appDbContext.Users.Where(x => x.Email == changePasswordViewModel.Email).SingleOrDefault();
                u.Password = EncryptPassword.TextToEncrypt(changePasswordViewModel.Password);
                _appDbContext.Entry(u).State = EntityState.Modified;//Updating the existing recrod in db set 
                _appDbContext.SaveChanges();//Updating  the record to the database
                ViewBag.Msg = "Password reseting process is completed successfully.";
                ViewBag.ActivateURL = "<a href=\"/account/login\">Click here to login again.</a>.";
                return View();
            }
            return View();
        }

        public IActionResult ProfileUpdate(string userId)
        {
            ViewBag.Countries = CountryEntity.GetAllCities();
            UserViewModel userView=_appDbContext.Users.Where(x=>x.Email==userId).Select(s=>new UserViewModel
            {
                Id=s.Id,
                UserName=s.UserName,
                Email=s.Email,
                Address=s.Address,
                Country=s.Country,
                Gender=s.Gender,
                DOB=s.DOB
            }).SingleOrDefault();
            return View(userView);
        }
        [HttpPost]
        public IActionResult ProfileUpdate(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserEntity userEntity = _appDbContext.Users.Where(x=>x.Id==userViewModel.Id).SingleOrDefault();
                    if(userEntity!=null)
                    {
                        userEntity.UserName = userViewModel.UserName;
                        userEntity.DOB = userViewModel.DOB;
                        userEntity.Address = userViewModel.Address;
                        userEntity.Country = userViewModel.Country;
                        userEntity.Gender = userViewModel.Gender;
                        _appDbContext.Update(userEntity);
                        _appDbContext.SaveChanges();
                        ViewBag.Msg = "Update successfully." + userViewModel.Email;
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Msg = "Failed update process" + e.Message;
                }
            }
            ViewBag.Countries = CountryEntity.GetAllCities();
            return View(userViewModel);
        }
        public IActionResult UpdatePassword(string userId)
        {
            UserViewModel userView = _appDbContext.Users.Where(x => x.Email == userId).Select(s => new UserViewModel
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email
            }).SingleOrDefault();
            return View(userView);
        }
        [HttpPost]
        public IActionResult UpdatePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            UserEntity user = _appDbContext.Users.Where(x => x.Email == updatePasswordViewModel.Email && x.Id == updatePasswordViewModel.Id).SingleOrDefault();
            try
            {             
                if (user.Password.Equals(EncryptPassword.TextToEncrypt(updatePasswordViewModel.CurrentPassword)))
                {
                    user.Password = EncryptPassword.TextToEncrypt(updatePasswordViewModel.NewPassword);
                    _appDbContext.Update(user);
                    _appDbContext.SaveChanges();
                    ViewBag.Msg = "Update new password for your account.";
                }
                else
                {
                    ViewBag.Msg = "Wrong current password.";
                }
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Failed update process" + e.Message; 
            }
            return View(new UserViewModel { Id=user.Id,Email=user.Email,UserName=user.UserName});
        }
    }
}
