using IbrahimPortfolio.Dto.MessageDtos;
using IbrahimPortfolio.WebUI.Models;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IbrahimPortfolio.WebUI.Controllers;

public class HomeController(IMessageApiService messageApiService) : Controller
{
    public IActionResult Index() => View(new CreateMessageDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(CreateMessageDto model)
    {
        if (!ModelState.IsValid) return View("Index", model);
        if (!await messageApiService.CreateAsync(model))
        {
            ModelState.AddModelError("", "Mesaj gönderilemedi. Lütfen daha sonra tekrar deneyin.");
            return View("Index", model);
        }
        TempData["MessageSuccess"] = "Mesajınız başarıyla gönderildi.";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [IgnoreAntiforgeryToken]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [IgnoreAntiforgeryToken]
    public IActionResult StatusCodePage(int id)
    {
        Response.StatusCode = id is >= 400 and <= 599 ? id : 404;
        ViewData["StatusCode"] = Response.StatusCode;
        return View();
    }
}
