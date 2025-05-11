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

        [HttpGet("/Index")]
        public async Task<IActionResult> Index()
        {
            var candidates = new List<CandidateModel>();
            var res = await _candidateService.GetAllAsync();

            if (res.IsSuccess && res.Value != null)
            {
                candidates = res.Value;
            }

            return View("Index", candidates);
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
            ViewData["skillList"] = GetSkillList();

            var countries = await GetCountryList();
            ViewData["countryList"] = countries;

            ViewData["FormIndex"] = index;

            return PartialView("_RegisterFormPartial", new CandidateModel());
        }

        [HttpPost("SaveCandidates")]
        public async Task<IActionResult> SaveCandidatesAsync([FromBody] List<CandidateModel> candidates)
        {
            if (candidates == null || !candidates.Any())
            {
                return Json(new { success = false, message = "No candidates received." });
            }

            var userId = 1;
            var res = await _candidateService.InsertBulkAsync(candidates, userId);

            if (res.IsFailed)
            {
                ViewBag.Message = $"An error occurred ! , {res.Errors.FirstOrDefault()?.Message}";
                return Json(new { success = false, message = "An error occurred." });
            }

            return Json(new { success = true, message = "Candidates saved successfully." });
        }


        [HttpPost("/SaveCandidate")]
        public async Task<IActionResult> Save([FromBody] CandidateModel entity)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", entity);
            }

            entity.CreatedBy = 1;

            var res = await _candidateService.InsertAsync(entity);

            if (res.IsFailed)
            {
                ViewBag.ErrorMessage = $"An error occurred ! , {res.Errors.FirstOrDefault()?.Message}";
                return Json(new { success = false, message = "An error occurred." });
            }

            return Json(new { success = true, message = "Candidates Updated successfully." });
        }

        [HttpGet("/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (id < 1)
            {
                ViewBag.ErrorMessage = "Invlaid Candidate Id.";
                return RedirectToAction(nameof(Index));
            }

            var deletedBy = 1;
            var res = await _candidateService.DeleteAsync(id, deletedBy);

            if (res.IsFailed)
            {
                ViewBag.Message(ErrorHelper.ConvertErrors(res.Errors));
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Candidate_Registration/{id}")]
        public async Task<IActionResult> CandidateRegistrationAsync(int id)
        {
            var entity = new CandidateModel();

            ViewData["skillList"] = GetSkillList();
            var countries = await GetCountryList();
            ViewData["countryList"] = countries;

            if (id > 0)
            {
                var res = await _candidateService.GetAsync(id);

                if (res.IsFailed)
                {
                    var error = res.Errors.FirstOrDefault()?.Message ?? "candidate not found !";
                    ViewBag.ErrorMessage = error;
                    return RedirectToAction(nameof(Index));
                }

                entity = res.Value;
            }
            else
            {
                entity.Skills = new List<string>();
            }

            return View("Create", entity);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}