using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CustomerController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}