using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Connections.Database;

namespace GestContact.Models
{
    public class ContactService
    {
        private readonly Connection _connection;

        public ContactService()
        {
            _connection = new Connection(SqlClientFactory.Instance, @"Data Source=VM-COREWIN\SQL2014DEV;Initial Catalog=GestContact;Integrated Security=True");
        }

        public IEnumerable<Contact> Get()
        {
            Command command = new Command("SELECT Id, LastName, FirstName, Email, Phone, BirthDate FROM Contact;");
            return _connection.ExecuteReader(command, (dr) => dr.ToContact());
        }

        public Contact Get(int id)
        {
            Command command = new Command("SELECT Id, LastName, FirstName, Email, Phone, BirthDate FROM Contact WHERE Id = @Id;");
            command.AddParameter("Id", id);
            return _connection.ExecuteReader(command, (dr) => dr.ToContact()).SingleOrDefault();
        }

        public int Insert(Contact entity)
        {
            Command command = new Command("CSP_AddContact", true);
            command.AddParameter("LastName", entity.LastName); 
            command.AddParameter("FirstName", entity.FirstName); 
            command.AddParameter("Email", entity.Email); 
            command.AddParameter("Phone", entity.Phone);
            command.AddParameter("BirthDate", entity.BirthDate);

            return (int)_connection.ExecuteScalar(command);
        }

        public bool Update(Contact entity)
        {
            Command command = new Command("CSP_UpdateContact", true);
            command.AddParameter("Id", entity.Id);
            command.AddParameter("LastName", entity.LastName);
            command.AddParameter("FirstName", entity.FirstName);
            command.AddParameter("Email", entity.Email);
            command.AddParameter("Phone", entity.Phone);
            command.AddParameter("BirthDate", entity.BirthDate);

            return _connection.ExecuteNonQuery(command) == 1;
        }

        public bool Delete(int id)
        {
            Command command = new Command("CSP_DeleteContact", true);
            command.AddParameter("Id", id);
            return _connection.ExecuteNonQuery(command) == 1;
        }
    }
}
