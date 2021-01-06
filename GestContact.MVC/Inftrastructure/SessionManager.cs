using GestContact.MVC.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestContact.MVC.Inftrastructure
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public Customer Customer
        {
            get
            {
                if (!_session.Keys.Contains(nameof(Customer)))
                    return null;

                return JsonConvert.DeserializeObject<Customer>(_session.GetString(nameof(Customer)));
            }

            set
            {
                _session.SetString(nameof(Customer), JsonConvert.SerializeObject(value));
            }
        }
    }
}
