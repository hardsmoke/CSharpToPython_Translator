using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Translator.Errors;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string csharpCode)
        {
            ViewData["csharpCode"] = csharpCode;

            List<Error> errors = Translator.Program.GetErrors(csharpCode);
            string errorsString = Translator.Program.GetErrorsAsString(errors);

            ViewData["errorLog"] = $"{errorsString}";

            if (errors.Count == 0)
            {
                ViewData["pythonCode"] = $"{Translator.Program.GetPythonCode(csharpCode)}";
            }


            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}