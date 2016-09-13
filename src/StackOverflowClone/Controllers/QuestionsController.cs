using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackOverflowClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StackOverflowClone.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;

        public QuestionsController (UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var questions = _db.Questions
                 .Include(q => q.User)
                 .Include(q => q.Responses)
                 .ToList();
            return View(questions);
        }

        public IActionResult Details(int id)
        {
            var question = _db.Questions
                .Include(q => q.User)
                .Include(q => q.Responses)
                .FirstOrDefault(q => q.Id == id);
            return View(question);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            question.User = currentUser;
            _db.Questions.Add(question);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
