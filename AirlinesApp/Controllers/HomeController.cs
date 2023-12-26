using AirlinesApp.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebApplication.RabbitMQ;
using System.Diagnostics;

namespace AirlinesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _rabbitMqService = new RabbitMQService();
        }

        public IActionResult Index()
        {
            _rabbitMqService.SendMessage("Home page");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}