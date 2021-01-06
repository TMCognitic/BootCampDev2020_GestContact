using GestContact.MVC.Models;

namespace GestContact.MVC.Inftrastructure
{
    public interface ISessionManager
    {
        Customer Customer { get; set; }
    }
}