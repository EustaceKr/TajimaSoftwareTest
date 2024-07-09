using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TajimaSoftwareTest.Models;

namespace TajimaSoftwareTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
