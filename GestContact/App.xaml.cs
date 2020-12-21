using GestContact.ViewModels.Messages;
using GestContact.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tools.Mvvm.Wpf.Mediator;

namespace GestContact
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Messenger<OpenWindow>.Instance.Register(OnNewOpenWindowMessage);
        }

        private void OnNewOpenWindowMessage(OpenWindow obj)
        {
            switch(obj.WindowName)
            {
                case "NewContact":
                    AddContactWindow w = new AddContactWindow();
                    w.ShowDialog();
                    break;
            }
        }
    }
}
