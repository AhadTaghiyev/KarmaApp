using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class OrderController : Controller
    {
        readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetAll());
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _orderService.Get(id));
        }

        public async Task<IActionResult> Accept(int id)
        {
            await _orderService.Accept(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Reject(int id)
        {
            await _orderService.Reject(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Compleet(int id)
        {
            await _orderService.Compleet(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

