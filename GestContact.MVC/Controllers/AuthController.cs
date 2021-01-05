using GestContact.MVC.Models.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tools.Connections.Database;

namespace GestContact.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConnection _connection;
        public AuthController()
        {
            _connection = new Connection(SqlClientFactory.Instance, @"Data Source=VM-COREWIN\SQL2014DEV;Initial Catalog=GestContact;Integrated Security=True;"); ;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginForm form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Command command = new Command("CSP_CheckCustomer", true);
                    command.AddParameter("Email", form.Email);
                    command.AddParameter("Passwd", form.Passwd);
                    var customer = _connection.ExecuteReader(command, (dr) => new { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"] }).SingleOrDefault();

                    if(customer is not null)
                    {
                        HttpContext.Session.SetInt32("Id", customer.Id);
                        HttpContext.Session.SetString("LastName", customer.LastName);
                        HttpContext.Session.SetString("FirstName", customer.FirstName);
                        return RedirectToAction("Index", "Contact");
                    }

                    ModelState.AddModelError("", "Email ou mot de passe invalide...");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterForm form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Command command = new Command("CSP_RegisterCustomer", true);
                    command.AddParameter("LastName", form.Nom);
                    command.AddParameter("FirstName", form.Prenom);
                    command.AddParameter("Email", form.Email);
                    command.AddParameter("Passwd", form.Passwd);
                    _connection.ExecuteNonQuery(command);

                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (!HttpContext.Session.GetInt32("Id").HasValue)
            {
                return RedirectToAction("Login");
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
