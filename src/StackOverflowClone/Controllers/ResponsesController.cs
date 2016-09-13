using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackOverflowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace StackOverflowClone.Controllers
{

    public class ResponsesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;

        public ResponsesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Response response)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            response.User = currentUser;
            _db.Responses.Add(response);
            _db.SaveChanges();
            return RedirectToAction("Details", "Questions", new { id = response.QuestionId });
        }
        [Authorize]
        public IActionResult Update(int responseId)
        {
            var thisResponse = _db.Responses.FirstOrDefault(r => r.Id == responseId);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (thisResponse.UserId == int.Parse(userId))
            {
                return View(thisResponse);
            }
            else
            {
                return RedirectToAction("Details", "Questions", new { id = thisResponse.QuestionId });
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Update(Response response)
        {
            _db.Entry(response).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Details", "Questions", new { id = response.QuestionId });
        }
        [Authorize]
        public IActionResult Delete(int responseId)
        {
            var thisResponse = _db.Responses.FirstOrDefault(r => r.Id == responseId);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (thisResponse.UserId == int.Parse(userId))
            {
                return View(thisResponse);
            }
            else
            {
                return RedirectToAction("Details", "Questions", new { id = thisResponse.QuestionId });
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Delete(Response response)
        {
            _db.Responses.Remove(response);
            _db.SaveChanges();
            return RedirectToAction("Details", "Questions", new { id = response.QuestionId });
        }
    }
}
