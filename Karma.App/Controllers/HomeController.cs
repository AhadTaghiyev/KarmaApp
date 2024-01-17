using System.Diagnostics;
using Karma.Core.DTOS;
using Karma.Service.ExternalServices.Interfaces;
using Karma.Service.Services.Implementations;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Karma.App.Controllers;

public class HomeController : Controller
{
    readonly IProductService _productService;
    readonly IEmailService _emailService;
    readonly IBasketService _basketService;

    public HomeController(IProductService productService, IEmailService emailService, IBasketService basketService)
    {
        _productService = productService;
        _emailService = emailService;
        _basketService = basketService;
    }

    public async Task<IActionResult> Index()
    {
        var basketJson = Request.Cookies["basket"];

        if (User.Identity.IsAuthenticated)
        {
            if (basketJson != null)
            {
                List<BasketDto> basketDtos = JsonConvert.DeserializeObject<List<BasketDto>>(basketJson);

                foreach (var item in basketDtos)
                {
                    await _basketService.AddBasket(item.Id, item.Count);
                }

                Response.Cookies.Delete("basket");
            }
        }

        return Json(await _productService.GetAllAsync());
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

