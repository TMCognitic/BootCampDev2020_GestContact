using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestContact.ViewModels.Messages
{
    public class OpenWindow
    {
        public string WindowName { get; init; }

        public OpenWindow(string windowName)
        {
            WindowName = windowName;
        }
    }
}
