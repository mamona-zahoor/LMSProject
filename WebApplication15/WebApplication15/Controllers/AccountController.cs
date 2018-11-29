using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication15.Models;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Data.SqlClient;
using WebApplication15;

namespace WebApplication15.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            string EmailId = "";
            string pwrd = "";
            DatabaseConnection Db = DatabaseConnection.getInstance();
            string data = "SELECT * from Admin";
            DatabaseConnection.getInstance().getConnection();
            SqlDataReader reader = DatabaseConnection.getInstance().getData(data);
            while (reader.Read())
            {
                EmailId = reader.GetString(0);
                pwrd = reader.GetString(1);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (model.Email == EmailId && model.Password == pwrd)
            {

                return RedirectToLocalMine(model, returnUrl);


            }
            else
            {
                switch (result)
                {

                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);

                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");

                        return View(model);
                }
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Registration_Num = model.Registration_Num, Designation = model.Designation };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    DatabaseConnection Db = DatabaseConnection.getInstance();
                    string add = "INSERT INTO Applied (Username, Email, Registeration_Num, Designation, Password, ResetPassword) values ('" + model.Name + "', '" + model.Email + "', '" + model.Registration_Num + "', '" + model.Designation + "', '" + model.Password + "', '')";
                    DatabaseConnection.getInstance().getConnection();
                    DatabaseConnection.getInstance().exectuteQuery(add);                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    using (MailMessage mailMes = new MailMessage())
                    {
                        string body = "";
                        mailMes.From = new MailAddress("cselibmansys@gmail.com");

                        mailMes.Subject = "CSE Library Membership Request";
                        if (model.RegisterAs == "Student")
                        {
                            body = "Name: " + model.Name + " \n Email : " + model.Email + " Registration Number: " + model.Registration_Num + "";
                        }
                        else if (model.RegisterAs == "Teacher")
                        {
                            body = "Name: " + model.Name + " \n Email : " + model.Email + " Designation : " + model.Designation + "";

                        }
                        mailMes.Body = body;
                        mailMes.IsBodyHtml = true;
                        mailMes.To.Add("librarian.csedept@gmail.com");
                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("cselibmansys@gmail.com", "LibManSys123");
                            smtp.EnableSsl = true;
                            smtp.Send(mailMes);
                        }

                        ViewBag.Message = string.Format("Your request has been sent!");
                        return RedirectToAction("Register", "Account");
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult MessageShow()
        {

            return View();
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
         [AllowAnonymous]
         [ValidateAntiForgeryToken]
         public ActionResult ForgotPassword(ForgotPasswordViewModel model)
         {
             if (ModelState.IsValid)
             {

                 bool areEqual = false;
                 string[] E = new string[30];
                 string[] M = new string[30];
                 string resetCode = GenerateRandomPassword(6);
                LMSEntities3 db = new LMSEntities3();
                 foreach (tbl_student s in db.tbl_student)
                 {
                     E = s.Email.Split(' ');
                     M = model.Email.Split(' ');
                     areEqual = E.SequenceEqual(M);
                     if (areEqual)
                     {
                         int n = s.ID;
                         db.tbl_student.Find(n).ResetPassword = resetCode;
                     }
                 }
                 if (!areEqual)
                 {
                     foreach (tbl_teacher t in db.tbl_teacher)
                     {
                         E = t.Email.Split(' ');
                         M = model.Email.Split(' ');
                         areEqual = E.SequenceEqual(M);
                         if (areEqual)
                         {
                             int n = t.ID;
                             db.tbl_teacher.Find(n).ResetPassword = resetCode;
                         }
                     }
                 }
                 if (!areEqual)
                 {
                     foreach (Admin A in db.Admins)
                     {
                         E = A.Email.Split(' ');
                         M = model.Email.Split(' ');
                         areEqual = E.SequenceEqual(M);
                         if (areEqual)
                         {
                             db.Admins.Find(model.Email).ResetPassword = resetCode;
                         }

                     }
                 }
                 db.SaveChanges();
                 if (areEqual)
                 {
                     SendEMail(model.Email, "Your verification code is " + resetCode + ".", "Verify your library account.");
                 }
                 else
                 {

                     ModelState.AddModelError("", "User is not registered in our system!!");
                     return View(model);
                 }

                 return View("ForgotPasswordConfirmation");



             }

             // If we got this far, something failed, redisplay form
             return View(model);

         }

    
        private void SendEMail(string emailid, string subject, string body)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("cselibmansys@gmail.com", "LibManSys123");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress("cselibmansys@gmail.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string code, string New, string Confirm)
        {
            string e = "";
            if (ModelState.IsValid)
            {
                if (code == null || New == null || Confirm == null)
                {
                    return View();
                }
                LMSEntities3 db = new LMSEntities3();
                bool areEqual = false, mail = false;
                foreach (tbl_student s in db.tbl_student)
                {
                    if (s.ResetPassword == code)
                    {
                        e = s.Email;
                        areEqual = true;
                    }
                }
                if (!areEqual)
                {
                    foreach (tbl_teacher t in db.tbl_teacher)
                    {
                        if (t.ResetPassword == code)
                        {
                            e = t.Email;
                            areEqual = true;
                        }
                    }
                }
                if (!areEqual)
                {
                    foreach (Admin A in db.Admins)
                    {
                        if (A.ResetPassword == code)
                        {
                            e = A.Email;
                            areEqual = true;
                            mail = true;
                        }
                    }
                }
                if (areEqual)
                {
                    if (New == Confirm)
                    {
                        if (mail)
                        {
                            db.Admins.Find(e).Password = Confirm;
                            db.SaveChanges();
                            return View("Login");
                        }
                        else
                        {
                            db.Applieds.Find(e).Password = Confirm;

                            return View("Login");
                        }
                    }

                }

                else
                {
                    return View();
                }
            }
            return View();

        }


    

        //
        // GET: /Account/ResetPassword
     //   [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool areEqual = false, done = false, Pwrd = false;
            string[] E = new string[1];
            string[] M = new string[1];
            string[] P = new string[1];
            string[] MP = new string[1];

            string resetCode = GenerateRandomPassword(6);
            LMSEntities3 db = new LMSEntities3();
            foreach (tbl_student s in db.tbl_student)
            {
                E = s.Email.Split(' ');
                M = model.Email.Split(' ');
                areEqual = E.SequenceEqual(M);
                if (areEqual)
                {
                    P = db.Applieds.Find(s.Email).Password.Split(' ');
                    MP = model.Old_Password.Split(' ');
                    Pwrd = MP[0].SequenceEqual(P[0]);
                    if (Pwrd)
                    {
                        db.Applieds.Find(model.Email).Password = model.New_Password;
                        done = true;
                    }
                }
            }
            db.SaveChanges();
            if (!done)
            {
                foreach (tbl_teacher t in db.tbl_teacher)
                {
                    E = t.Email.Split(' ');
                    M = model.Email.Split(' ');
                    P = db.Applieds.Find(model.Email).Password.Split(' ');
                    MP = model.Old_Password.Split(' ');
                    Pwrd = P.SequenceEqual(MP);
                    areEqual = E.SequenceEqual(M);
                    if (areEqual)
                    {
                        P = db.Applieds.Find(t.Email).Password.Split(' ');
                        MP = model.Old_Password.Split(' ');
                        Pwrd = MP[0].SequenceEqual(P[0]);
                        if (Pwrd)
                        {
                            done = true;
                            db.Applieds.Find(model.Email).Password = model.New_Password;
                        }
                    }
                }
            }
            db.SaveChanges();
            if (!done)
            {
                foreach (Admin A in db.Admins)
                {
                    E = A.Email.Split(' ');
                    M = model.Email.Split(' ');
                    areEqual = E.SequenceEqual(M);
                    if (areEqual)
                    {
                        done = true;
                        db.Admins.Find(model.Email).Password = model.New_Password;
                    }

                }
            }
            db.SaveChanges();
            if (!done)
            {
                ModelState.AddModelError("", "User is not registered in our system!!");
                return View(model);
            }

            return View("Login");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public object ClientScript { get; private set; }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocalMine(LoginViewModel model, string returnUrl)
        {


            return RedirectToAction("Register", "Account");

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}