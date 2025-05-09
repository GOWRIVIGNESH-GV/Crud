using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crud.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crud.Controllers;

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

    private List<SelectListItem> GetCountryList()
    {
        var countries = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "1",
                Text = "India"
            },
             new SelectListItem()
            {
                Value = "2",
                Text = "Russia"
            },
             new SelectListItem()
            {
                Value = "3",
                Text = "America"
            }
        };
        return countries;
    }
    private List<string> GetSkillList()
    {
        var skills = new List<string>
        {
"C","C++","Java","C#","React"
        };
        return skills;
    }
    public IActionResult GetRegisterForm(int index)
    {
        var model = new CandidateModel();

        ViewBag.skillList = GetSkillList();
        ViewBag.CountryList = GetCountryList();
        ViewData["FormIndex"] = index;
        return PartialView("_RegisterFormPartial", model);
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
