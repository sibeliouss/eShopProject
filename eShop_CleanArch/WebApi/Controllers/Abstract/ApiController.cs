using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Abstract;

//DRY : Don't repeat yourself!
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")] //attribute // Kullanılmasını istemediğimiz endpointte [AllowAnonymous] yazılır.
public abstract class ApiController : ControllerBase
{
}