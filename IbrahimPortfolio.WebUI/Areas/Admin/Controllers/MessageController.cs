using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class MessageController(IMessageApiService messageApiService) : AdminBaseController
{
    public async Task<IActionResult> Index() => View(await messageApiService.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var message = await messageApiService.GetByIdAsync(id);
        return message == null ? StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404) : View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Open(int id)
    {
        if (!await messageApiService.MarkAsReadAsync(id))
            TempData["ErrorMessage"] = "Mesaj okundu olarak işaretlenemedi.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await messageApiService.MarkAsReadAsync(id);
        TempData[result ? "SuccessMessage" : "ErrorMessage"] = result
            ? "Mesaj okundu olarak işaretlendi." : "Mesaj güncellenemedi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await messageApiService.DeleteAsync(id);
        TempData[result ? "SuccessMessage" : "ErrorMessage"] = result
            ? "Mesaj başarıyla silindi." : "Mesaj silinemedi.";
        return RedirectToAction(nameof(Index));
    }
}

