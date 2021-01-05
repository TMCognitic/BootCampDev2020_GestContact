using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestContact.ViewModels
{
    //Implemente l'IOC et Injection de dépendance
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //Initialiser l'IOC
        }

        public ContactsViewModel Contacts
        {
            get
            {
                //Fait appel à IOC pour retourner le ViewModel
                return new ContactsViewModel();
            }
        }
    }
}
