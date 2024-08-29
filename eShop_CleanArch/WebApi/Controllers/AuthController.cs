using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}