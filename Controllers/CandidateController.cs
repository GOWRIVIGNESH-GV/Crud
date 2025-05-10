using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Crud.Helper;
using Crud.Models;
using Crud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Crud.Controllers
{
    [Route("[controller]")]
    public class CandidateController : Controller
    {
        private readonly ILogger<CandidateController> _logger;
        private readonly ICandidateService _candidateService;


        public CandidateController(ILogger<CandidateController> logger, ICandidateService candidateService)
        {
            _logger = logger;
            _candidateService = candidateService;
        }

        public async Task<IActionResult> Index()
        {
            var candidates = new List<CandidateModel>();
            var res = await _candidateService.GetAllAsync();

            if (res.IsSuccess && res.Value != null)
            {
                candidates = res.Value;
            }

            return View(candidates);
        }

        [HttpGet("/Registration")]
        public IActionResult Registration()
        {
            return View();
        }
        private async Task<List<SelectListItem>> GetCountryList()
        {

            // var countries = new List<SelectListItem>();


            var countries = Shared.SessionData._CountryList;

            if (countries != null && countries.Count > 0)
            {
                return countries.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.CountryName }).ToList();
            }

            var result = await _candidateService.GetCountries();

            if (result.IsSuccess && result.Value != null)
            {
                Shared.SessionData._CountryList = result.Value;
                return Shared.SessionData._CountryList.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.CountryName }).ToList();
            }


            return new List<SelectListItem>();


            //     var countries = new List<SelectListItem>
            // {
            //     new SelectListItem()
            //     {
            //         Value = "1",
            //         Text = "India"
            //     },
            //      new SelectListItem()
            //     {
            //         Value = "2",
            //         Text = "Russia"
            //     },
            //      new SelectListItem()
            //     {
            //         Value = "3",
            //         Text = "America"
            //     }
            // };
            // return countries;
        }
        private List<string> GetSkillList()
        {
            var skills = new List<string>
        {
            "C","C++","Java","C#","React"
        };
            return skills;
        }
        [HttpGet("/GetRegistrationForm")]
        public async Task<IActionResult> GetRegisterForm(int index)
        {
            var model = new CandidateModel();

            ViewBag.skillList = GetSkillList();

            var countries = await GetCountryList();
            ViewBag.CountryList = countries;
            ViewData["FormIndex"] = index;
            return PartialView("_RegisterFormPartial", model);
        }

        [HttpPost("SaveCandidates")]
        public IActionResult SaveCandidates([FromBody] List<CandidateModel> candidates)
        {
            if (candidates == null || !candidates.Any())
            {
                return Json(new { success = false, message = "No candidates received." });
            }

            // TODO: Save candidates to the database (or any other processing)
            foreach (var candidate in candidates)
            {
                // Log or process each candidate
                Console.WriteLine($"Saving candidate: {candidate.Name}, Skills: {string.Join(",", candidate.Skills)}");
            }

            return Json(new { success = true, message = "Candidates saved successfully." });
        }
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(CandidateModel candidate)
        {
            //     bool isServiceAvailable = _apiService.CheckApiHealth();

            //     if (!isServiceAvailable)
            //     {
            //         return RedirectToAction("ServerUnavailableError", "Error");
            //     }
            //     if (!ModelState.IsValid)
            //     {
            //         user.UserActiveStatusList = GlobalAccess.GetActiveStatusList();
            //         user.UserRoleList = GlobalAccess.GetUserRoleList();
            //         return View("Create", user);
            //     }

            //     var userId = _userContextService.GetUserId();
            //     user.ChangePassword = true;
            //     if (string.IsNullOrEmpty(userId))
            //     {
            //         return Unauthorized();
            //     }

            //     user.CreatedBy = Convert.ToInt32(userId);

            //     var token = _userContextService.GetJwtToken();

            //     if (string.IsNullOrEmpty(token))
            //     {
            //         return Unauthorized();
            //     }

            //     var res = await _userService.InsertAsync(user, token);

            //     if (res.IsFailed)
            //     {
            //         ViewBag.ErrorMessage = "An error occured while trying to edit user data";
            //         user.UserActiveStatusList = GlobalAccess.GetActiveStatusList();
            //         user.UserRoleList = GlobalAccess.GetUserRoleList();
            //         return View("Create", user);
            //     }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet("/Delete/{id}")]
        public async Task<IActionResult> Delete(int candidateId)
        {

            if (candidateId < 1)
            {
                ViewBag.ErrorMessage = "Invlaid Candidate Id.";
                return RedirectToAction(nameof(Index));
            }

            var deletedBy = 1;
            var res = await _candidateService.DeleteAsync(candidateId, deletedBy);

            if (res.IsFailed)
            {
                ViewBag.Message(ErrorHelper.ConvertErrors(res.Errors));
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}