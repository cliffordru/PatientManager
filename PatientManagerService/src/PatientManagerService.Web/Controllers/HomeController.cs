using System.Diagnostics;
using PatientManagerService.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PatientManagerService.Web.Controllers;

/// <summary>
/// A sample MVC controller that uses views.
/// Razor Pages are a good alternative, since the behavior, viewmodel, and view are all in one place,
/// rather than spread between 3 different folders in your Web project
/// </summary>
public class HomeController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
