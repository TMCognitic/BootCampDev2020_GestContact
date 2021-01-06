using GestContact.MVC.Inftrastructure;
using GestContact.MVC.Models;
using GestContact.MVC.Models.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ISessionManager _sessionManager;
        private readonly ILogger _logger;

        public AuthController(IConnection connection, ISessionManager sessionManager, ILogger<AuthController> logger)
        {           
            _connection = connection;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.HashCode = _connection.GetHashCode();
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
                    Customer customer = _connection.ExecuteReader(command, (dr) => new Customer { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"] }).SingleOrDefault();

                    if(customer is not null)
                    {
                        _sessionManager.Customer = customer;
                        return RedirectToAction("Index", "Contact");
                    }

                    ModelState.AddModelError("", "Email ou mot de passe invalide...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("", "Une erreur est survenue");
                //ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.HashCode = _connection.GetHashCode();
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
        [AuthRequired]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
