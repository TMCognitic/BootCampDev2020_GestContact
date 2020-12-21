using GestContact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tools.Mvvm.Wpf.Commands;
using Tools.Mvvm.Wpf.Mediator;
using Tools.Mvvm.Wpf.ViewModels;

namespace GestContact.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private readonly ContactService _service;
        private readonly Contact _entity;
        private string _lastName, _firstName, _email, _phone;
        private DateTime? _birthDate;
        private ICommand _updateCommand;
        private ICommand _cancelCommand;
        private ICommand _deleteCommand;

        public int Id
        {
            get
            {
                return _entity.Id;
            }
        }

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

        public ICommand UpdateCommand
        {
            get
            {
                return _updateCommand ??= new DelegateCommand(Update, CanUpdate);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(Cancel, CanUpdate);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new DelegateCommand(Delete, null);
            }
        }

        private void Delete()
        {
            _service.Delete(Id);
            Messenger<Message<ContactViewModel>>.Instance.Send("Contacts", new Message<ContactViewModel>(this, CrudAction.Delete));            
        }


        private void Cancel()
        {
            LastName = _entity.LastName;
            FirstName = _entity.FirstName;
            Email = _entity.Email;
            Phone = _entity.Phone;
            BirthDate = _entity.BirthDate;
        }

        private bool CanUpdate()
        {
            return LastName != _entity.LastName ||
                FirstName != _entity.FirstName ||
                Email != _entity.Email ||
                Phone != _entity.Phone ||
                BirthDate.Value != _entity.BirthDate;
        }

        private void Update()
        {
            _entity.LastName = LastName;
            _entity.FirstName = FirstName;
            _entity.Email = Email;
            _entity.Phone = Phone;
            _entity.BirthDate = BirthDate.Value;

            _service.Update(_entity);
        }

        public ContactViewModel(Contact entity)
        {            
            _service = new ContactService();

            _entity = entity;
            LastName = entity.LastName;
            FirstName = entity.FirstName;
            Email = entity.Email;
            Phone = entity.Phone;
            BirthDate = entity.BirthDate;
        }
    }
}
