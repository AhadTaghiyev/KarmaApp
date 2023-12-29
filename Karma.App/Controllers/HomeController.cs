using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Karma.App.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }


    public IActionResult ChangeColor(string name)
    {
        Response.Cookies.Append("color",name);
        return RedirectToAction(nameof(Index));
    }

}

