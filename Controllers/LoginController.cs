using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IDZ.Models.Entities;
using IDZ.Models.ViewModels;

namespace IDZ.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    USER user = db.USER.Where(
                        u => u.LOGIN == userVM.Login).FirstOrDefault();
                    if (user != null)
                    {
                        string passwordHash = ReturnHashCode(userVM.Password +
                            user.SALT.ToString().ToUpper());
                        if (passwordHash == user.PASSWORD_HASH)
                        {
                            string userRole = "";
                            switch (user.ROLE_ID)
                            {
                                case 1:
                                    userRole = "Admin";
                                    break;
                                case 2:
                                    userRole = "Patient";
                                    DataController.getPatientId(user.ID);
                                    break;
                            }
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1,
                                user.LOGIN,
                                DateTime.Now,
                                DateTime.Now.AddDays(1),
                                false,
                                userRole
                            );
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpContext.Response.Cookies.Add(new HttpCookie(
                                FormsAuthentication.FormsCookieName,
                                encryptedTicket
                                ));
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            ViewBag.Error = "Неверный логин или пароль";
            return View(userVM);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private string ReturnHashCode(string loginAndSalt)
        {
            string hash = "";
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(loginAndSalt));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                hash = sBuilder.ToString().ToUpper();
            }
            return hash;
        }
    }
}