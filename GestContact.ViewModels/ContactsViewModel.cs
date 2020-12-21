using GestContact.Models;
using GestContact.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tools.Mvvm.Wpf;
using Tools.Mvvm.Wpf.Commands;
using Tools.Mvvm.Wpf.Mediator;
using Tools.Mvvm.Wpf.ViewModels;

namespace GestContact.ViewModels
{
    public class ContactsViewModel : CollectionViewModelBase<ContactViewModel>
    {
        private readonly ContactService _service;
        private string _lastName, _firstName, _email, _phone;
        private DateTime? _birthDate;

        private ICommand _insertCommand;
        private ICommand _openNewCommand;

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                SetValue(ref _lastName, value);
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                SetValue(ref _firstName, value);
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                SetValue(ref _email, value);
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }

            set
            {
                SetValue(ref _phone, value);
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }

            set
            {
                SetValue(ref _birthDate, value);
            }
        }

        public ICommand InsertCommand
        {
            get
            {
                return _insertCommand ??= new DelegateCommand(Insert, CanInsert);
            }
        }

        public ICommand OpenNewCommand
        {
            get
            {
                return _openNewCommand ??= new DelegateCommand(OpenAddContactWindow, null);
            }
        }

        private void OpenAddContactWindow()
        {
            Messenger<OpenWindow>.Instance.Send(new OpenWindow("NewContact"));
        }

        private bool CanInsert()
        {
            return !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrWhiteSpace(Phone) &&
                BirthDate.HasValue;
        }

        private void Insert()
        {
            Contact contact = new Contact() { LastName = LastName, FirstName = FirstName, Email = Email, Phone = Phone, BirthDate = BirthDate.Value };
            contact.Id = _service.Insert(contact);
            Items.Add(new ContactViewModel(contact));

            LastName = FirstName = Email = Phone = null;
            BirthDate = null;
        }

        public ContactsViewModel()
        {
            _service = new ContactService();
            Messenger<Message<ContactViewModel>>.Instance.Register("Contacts", OnDeleteContact);
        }

        private void OnDeleteContact(Message<ContactViewModel> message)
        {
            switch(message.Action)
            {
                case CrudAction.Delete:
                    Items.Remove(message.Data);
                    break;
            }            
        }

        protected override ObservableCollection<ContactViewModel> LoadItems()
        {
            return new ObservableCollection<ContactViewModel>(_service.Get().Select(c => new ContactViewModel(c)));
        }
    }
}
