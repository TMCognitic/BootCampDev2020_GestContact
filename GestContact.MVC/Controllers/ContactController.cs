using GestContact.MVC.Inftrastructure;
using GestContact.MVC.Models;
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
    [AuthRequired]
    public class ContactController : Controller
    {
        private readonly IConnection _connection;
        private readonly ISessionManager _sessionManager;

        public ContactController(IConnection connection, ISessionManager sessionManager)
        {
            _connection = connection;
            _sessionManager = sessionManager;
        }

        // GET: ContactController
        public ActionResult Index()
        {
            Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId");
            command.AddParameter("CustomerId", _sessionManager.Customer.Id);

            return View(_connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = _sessionManager.Customer.Id }));
        }

        // GET: ContactController/Details/5
        public ActionResult Details(int id)
        {
            Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
            command.AddParameter("CustomerId", _sessionManager.Customer.Id);
            command.AddParameter("id", id);
            Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = _sessionManager.Customer.Id }).SingleOrDefault();

            if (contact is null)
                return RedirectToAction("Index");

            return View(contact);
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContactForm form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Command command = new Command("CSP_AddContact", true);
                    command.AddParameter("LastName", form.LastName);
                    command.AddParameter("FirstName", form.FirstName);
                    command.AddParameter("Email", form.Email);
                    command.AddParameter("Phone", form.Phone);
                    command.AddParameter("BirthDate", form.BirthDate);
                    command.AddParameter("CustomerId", _sessionManager.Customer.Id);
                    _connection.ExecuteNonQuery(command);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //ViewBag.Error = ex.Message;
            }

            return View(form);
        }

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
            command.AddParameter("CustomerId", _sessionManager.Customer.Id);
            command.AddParameter("id", id);
            Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = _sessionManager.Customer.Id }).SingleOrDefault();

            if (contact is null)
                return RedirectToAction("Index");

            return View(new UpdateContactForm() { Id = contact.Id, LastName = contact.LastName, FirstName = contact.FirstName, Email = contact.Email, Phone = contact.Phone, BirthDate = contact.BirthDate });
        }

        // POST: ContactController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateContactForm form)
        {
            if (id != form.Id)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                try
                {
                    Command command = new Command("CSP_UpdateContact", true);
                    command.AddParameter("Id", id);
                    command.AddParameter("LastName", form.LastName);
                    command.AddParameter("FirstName", form.FirstName);
                    command.AddParameter("Email", form.Email);
                    command.AddParameter("Phone", form.Phone);
                    command.AddParameter("BirthDate", form.BirthDate);
                    command.AddParameter("CustomerId", _sessionManager.Customer.Id);
                    _connection.ExecuteNonQuery(command);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    //ViewBag.Error = ex.Message;
                }
            }

            return View(form);
        }

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
            command.AddParameter("CustomerId", _sessionManager.Customer.Id);
            command.AddParameter("id", id);
            Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = _sessionManager.Customer.Id }).SingleOrDefault();

            if (contact is null)
                return RedirectToAction("Index");

            return View(contact);
        }

        // POST: ContactController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Command command = new Command("CSP_DeleteContact", true);
                command.AddParameter("Id", id);
                command.AddParameter("CustomerId", _sessionManager.Customer.Id);
                _connection.ExecuteNonQuery(command);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //ViewBag.Error = ex.Message;
            }

            return View(collection);
        }
    }
}
