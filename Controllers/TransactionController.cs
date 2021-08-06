using BankLib;
using BankRepository;
using CustomerBankApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerBankApplication.Controllers
{
    public class TransactionController : Controller
    {
        private IBankRepository _bank;
        public TransactionController(IBankRepository bank)
        {
            _bank = bank;
        }
        // GET: Transaction
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(TransactionPageVM vm)
        {
            if (Session["username"] == null && Session["password"] == null)
            {
                ModelState.AddModelError("", "Please Login");
                return View(vm);
            }
            if (!this.ModelState.IsValid)
            {
                return View(vm);
            }
            try
            {
                _bank.DoTrasanction(Session["username"].ToString(), Session["password"].ToString(), vm.Amount, vm.TransactionType);
                ViewBag.transactionStatus = "Transaction Succesfull";
            }
            catch
            {
                ViewBag.transactionStatus = "Transaction failed";
            }
            return View();
        }
        public ActionResult Transactions()
        {
            List<BankTransaction> transactionsDone = _bank.GetTransactions(Session["username"].ToString());
            return View(transactionsDone);
        }
        public void GenerateCsv()
        {
            StringWriter sw = new StringWriter();
            List<BankTransaction> transactionsDone = _bank.GetTransactions(Session["username"].ToString());

            sw.WriteLine("\"Amount\",\"Transaction Type\",\"Date\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Transaction.csv");
            Response.ContentType = "text/csv";

            foreach (var transaction in transactionsDone)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"",
                                           transaction.Amount,
                                           transaction.TransactionType,
                                           transaction.Date
                                           ));
            }

            Response.Write(sw.ToString());

            Response.End();
        }
    }
}