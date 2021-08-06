using BankRepository;
using CustomerBankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerBankApplication.Controllers
{
    public class RegistrationController : Controller
    {
        private IBankRepository _bank;

        public RegistrationController(IBankRepository bank)
        {
            _bank = bank;
        }
        // GET: Registration
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ResgistrationPageVM vm)
        {
            if (!this.ModelState.IsValid)
            {
                return View(vm);
            }
            _bank.Register(vm.Name, vm.Balance, vm.Password);
            return RedirectToAction("Index", "Login");
        }
    }
}