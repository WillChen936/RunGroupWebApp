using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Repositories.Interfaces;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _context;

        public RaceController(IRaceRepository context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _context.GetAllAsync();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var race = await _context.GetByIdAsync(id);
            return View(race);
        }
    }
}
