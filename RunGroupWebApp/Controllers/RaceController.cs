using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunGroupWebApp.Repositories.Interfaces;
using RunGroupWebApp.Services.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoservice _photoservice;


        public RaceController(IRaceRepository raceRepository, IPhotoservice photoservice)
        {
            _raceRepository = raceRepository;
            _photoservice = photoservice;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAllAsync();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RaceViewModel raceViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoservice.AddPhotoAsync(raceViewModel.Image);
                var race = new Race
                {
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceViewModel.Address.Street,
                        City = raceViewModel.Address.City,
                        State = raceViewModel.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Photo upload failed");
            return View(raceViewModel);
        }
    }
}
