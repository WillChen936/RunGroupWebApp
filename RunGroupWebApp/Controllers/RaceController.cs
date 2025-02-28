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
        private readonly IPhotoservice _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoservice photoservice, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoservice;
            _httpContextAccessor = httpContextAccessor;
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
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = currUserId };
            return View(createRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel createRaceViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(createRaceViewModel.Image);
                var race = new Race
                {
                    AppUserId = createRaceViewModel.AppUserId,
                    Title = createRaceViewModel.Title,
                    Description = createRaceViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = createRaceViewModel.Address.Street,
                        City = createRaceViewModel.Address.City,
                        State = createRaceViewModel.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Photo upload failed");
            return View(createRaceViewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            var createRaceViewModel = new RaceEditViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };

            return View(createRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, RaceEditViewModel createRaceViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", createRaceViewModel);
            }

            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(createRaceViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(createRaceViewModel.Image);
                var race = new Race
                {
                    Id = id,
                    Title = createRaceViewModel.Title,
                    Description = createRaceViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = createRaceViewModel.AddressId,
                    Address = createRaceViewModel.Address,
                    RaceCategory = createRaceViewModel.RaceCategory
                };

                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(createRaceViewModel);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");
            return View(raceDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");

            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}
