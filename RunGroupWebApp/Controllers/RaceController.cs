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


        public RaceController(IRaceRepository raceRepository, IPhotoservice photoservice)
        {
            _raceRepository = raceRepository;
            _photoService = photoservice;
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
                var result = await _photoService.AddPhotoAsync(raceViewModel.Image);
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
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            var raceViewModel = new RaceEditViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };

            return View(raceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, RaceEditViewModel raceViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceViewModel);
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
                    return View(raceViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceViewModel.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceViewModel.AddressId,
                    Address = raceViewModel.Address,
                    RaceCategory = raceViewModel.RaceCategory
                };

                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceViewModel);
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
