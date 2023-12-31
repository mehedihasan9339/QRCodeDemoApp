using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design.Internal;
using QRCodeDemoApp.Helper;
using QRCodeDemoApp.Models;
using System.Diagnostics;
using ZXing;

namespace QRCodeDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QrCodeGenerator _qrCodeGenerator;
        public HomeController(ILogger<HomeController> logger, QrCodeGenerator qrCodeGenerator)
        {
            _logger = logger;
            _qrCodeGenerator = qrCodeGenerator;
        }

        public IActionResult Index()
        {
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



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GenerateQRCodeImage(QRCodeViewModel model)
        {
            string data = $"Name: {model.Name}\nPhone: {model.Phone}\nEmail: {model.Email}\nUrl: {model.Url}\nAddress: {model.Address}\nOccupation: {model.Occupation}\nDateOfBirth: {model.DateOfBirth}\nGender: {model.Gender}\nAdditionalField: {model.AdditionalField}";


            string qrCode = _qrCodeGenerator.GenerateAndSaveQrCode(data);

            return Json(qrCode);
        }
    }
}
