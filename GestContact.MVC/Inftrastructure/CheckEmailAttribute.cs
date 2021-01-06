using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tools.Connections.Database;

namespace GestContact.MVC.Inftrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckEmailAttribute : ValidationAttribute
    {
        //public override bool IsValid(object value)
        //{
        //    if(value is string email)
        //    {                
        //        Connection connection = new Connection(SqlClientFactory.Instance, @"Data Source=VM-COREWIN\SQL2014DEV;Initial Catalog=GestContact;Integrated Security=True;");
        //        Command command = new Command("CSP_ExistEmail", true);
        //        command.AddParameter("Email", email);
        //        if (!(bool)connection.ExecuteScalar(command))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            ErrorMessage = "Email already use!!";
        //        }                
        //    }
        //    return false;
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            if (value is string email)
            {
                IConnection connection = (IConnection)validationContext.GetService(typeof(IConnection));
                Command command = new Command("CSP_ExistEmail", true);
                command.AddParameter("Email", email);
                if ((bool)connection.ExecuteScalar(command))
                {
                    return new ValidationResult("Email already use!!");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("It's not a string");
        }
    }
}
