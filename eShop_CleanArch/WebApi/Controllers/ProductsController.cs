using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ProductsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}