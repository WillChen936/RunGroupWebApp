using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _context;

        public ClubController(IClubRepository context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _context.GetAllAsync();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var club = await _context.GetByIdAsync(id);
            return View(club);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if(!ModelState.IsValid)
            {
                return View(club);
            }
            _context.Add(club);
            return RedirectToAction("Index");
        }
    }
}
