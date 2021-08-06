using BankRepository;
using CustomerBankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerBankApplication.Controllers
{
    public class LoginController : Controller
    {
        private IBankRepository _Loginbank;

        public LoginController(IBankRepository bank)
        {
            _Loginbank = bank;
        }
        
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginPageVM vm)
        {
            if (!this.ModelState.IsValid)
            {
                return View(vm);

            }
            var name = _Loginbank.Login(vm.Name, vm.Password);
            if (name != null && name != "")
            {
                Session["username"] = vm.Name;
                Session["password"] = vm.Password;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Login Error", "Wrong username or password");
            return View(vm);
        }

        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("password");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register()
        {
            return RedirectToAction("Index", "Registration");
        }
    }
}