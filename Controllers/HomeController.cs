using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crud.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Crud.Services;
using System.Threading.Tasks;

namespace Crud.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICandidateService _candidateService;

    public HomeController(ILogger<HomeController> logger, ICandidateService candidateService)
    {
        _logger = logger;
        _candidateService = candidateService;
    }

    public IActionResult Index()
    {
        // var candidates = new List<CandidateModel>();
        // var res = await _candidateService.GetAllAsync();

        // if (res.IsSuccess && res.Value != null)
        // {
        //     candidates = res.Value;
        // }

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
}
