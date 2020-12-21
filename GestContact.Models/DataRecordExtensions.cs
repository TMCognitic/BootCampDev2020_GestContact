using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestContact.Models
{
    static class DataRecordExtensions
    {
        internal static Contact ToContact(this IDataRecord record)
        {
            return new Contact()
            {
                Id = (int)record["Id"],
                LastName = (string)record["LastName"],
                FirstName = (string)record["FirstName"],
                Email = (string)record["Email"],
                Phone = (string)record["Phone"],
                BirthDate = (DateTime)record["BirthDate"]
            };
        }
    }
}
