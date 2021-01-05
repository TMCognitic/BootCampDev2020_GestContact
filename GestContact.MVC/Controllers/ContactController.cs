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
    public class ContactController : Controller
    {
        private readonly IConnection _connection;
        public ContactController()
        {
            _connection = new Connection(SqlClientFactory.Instance, @"Data Source=VM-COREWIN\SQL2014DEV;Initial Catalog=GestContact;Integrated Security=True;"); ;
        }

        // GET: ContactController
        public ActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Id").HasValue)
            {
                Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId");
                command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);

                return View(_connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = HttpContext.Session.GetInt32("Id").Value }));
            }

            return RedirectToAction("Login", "Auth");
        }

        // GET: ContactController/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
            {
                Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
                command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
                command.AddParameter("id", id);
                Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = HttpContext.Session.GetInt32("Id").Value }).SingleOrDefault();

                if (contact is null)
                    return RedirectToAction("Index");

                return View(contact);
            }

            return RedirectToAction("Login", "Auth");
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
            {
                return View();
            }

            return RedirectToAction("Login", "Auth");
        }

        // POST: ContactController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContactForm form)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
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
                        command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
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

            return RedirectToAction("Login", "Auth");
        }

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
            {
                Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
                command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
                command.AddParameter("id", id);
                Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = HttpContext.Session.GetInt32("Id").Value }).SingleOrDefault();

                if (contact is null)
                    return RedirectToAction("Index");

                return View(new UpdateContactForm() { Id = contact.Id, LastName = contact.LastName, FirstName = contact.FirstName, Email = contact.Email, Phone = contact.Phone, BirthDate = contact.BirthDate });
            }

            return RedirectToAction("Login", "Auth");
        }

        // POST: ContactController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateContactForm form)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
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
                        command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
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

            return RedirectToAction("Login", "Auth");
        }

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
            {
                Command command = new Command("Select Id, LastName, FirstNAme, Email, Phone, BirthDate from Contact where CustomerId = @CustomerId and Id = @Id");
                command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
                command.AddParameter("id", id);
                Contact contact = _connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"], Email = (string)dr["Email"], Phone = (string)dr["Phone"], BirthDate = (DateTime)dr["BirthDate"], CustomerId = HttpContext.Session.GetInt32("Id").Value }).SingleOrDefault();

                if (contact is null)
                    return RedirectToAction("Index");

                return View(contact);
            }

            return RedirectToAction("Login", "Auth");
        }

        // POST: ContactController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("Id").HasValue)
            {
                try
                {                    
                    Command command = new Command("CSP_DeleteContact", true);
                    command.AddParameter("Id", id);
                    command.AddParameter("CustomerId", HttpContext.Session.GetInt32("Id").Value);
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

            return RedirectToAction("Login", "Auth");
        }
    }
}
