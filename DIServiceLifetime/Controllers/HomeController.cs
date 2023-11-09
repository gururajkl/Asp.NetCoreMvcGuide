using DIServiceLifetime.Models;
using DIServiceLifetime.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace DIServiceLifetime.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScopedGUIDService scoped1;
        private readonly IScopedGUIDService scoped2;

        private readonly ISingletonGUIDService singleton1;
        private readonly ISingletonGUIDService singleton2;

        private readonly ITransientGUIDService transient1;
        private readonly ITransientGUIDService transient2;

        public HomeController(IScopedGUIDService scoped1, IScopedGUIDService scoped2, ISingletonGUIDService singleton1, ISingletonGUIDService singleton2, ITransientGUIDService transient1, ITransientGUIDService transient2)
        {
            this.scoped1 = scoped1;
            this.scoped2 = scoped2;
            this.singleton1 = singleton1;
            this.singleton2 = singleton2;
            this.transient1 = transient1;
            this.transient2 = transient2;
        }

        public IActionResult Index()
        {
            StringBuilder message = new StringBuilder();

            message.Append($"Transient 1: {transient1.GetGUID()}\n");
            message.Append($"Transient 2: {transient2.GetGUID()}\n");

            message.Append($"Scoped 1: {scoped1.GetGUID()}\n");
            message.Append($"Scoped 2: {scoped2.GetGUID()}\n");

            message.Append($"Singleton 1: {singleton1.GetGUID()}\n");
            message.Append($"Singleton 2: {singleton2.GetGUID()}\n");

            return Ok(message.ToString());
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