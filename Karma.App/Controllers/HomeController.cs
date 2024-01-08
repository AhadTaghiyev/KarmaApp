using System.Diagnostics;
using Karma.Service.ExternalServices.Interfaces;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Karma.App.Controllers;

public class HomeController : Controller
{
    readonly IProductService _productService;
    readonly IEmailService _emailService;

    public HomeController(IProductService productService, IEmailService emailService)
    {
        _productService = productService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAllAsync());
    }


    public async Task<IActionResult> SendEmail()
    {
        await _emailService.SendEmail("taghiyev.ahad@gmail.com","Test","<h1>ELnur will go to east</h1>");

        return Json("ok");
    }

    public IActionResult ChangeColor(string name)
    {
        Response.Cookies.Append("color",name);
        return RedirectToAction(nameof(Index));
    }

}

