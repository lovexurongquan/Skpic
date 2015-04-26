using System.Web.Mvc;
using Skpic.Main.Web.Models;

namespace Skpic.Main.Web.Controllers
{
    public class AccountController : Controller
    {
 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }   
        
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {


            return View();
        }
	}
}