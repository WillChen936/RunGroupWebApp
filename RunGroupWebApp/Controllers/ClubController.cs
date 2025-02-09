using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;
using RunGroupWebApp.Services.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoservice _photoservice;

        public ClubController(IClubRepository clubRepository, IPhotoservice photoservice)
        {
            _clubRepository = clubRepository;
            _photoservice = photoservice;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAllAsync();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ClubViewModel clubViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoservice.AddPhotoAsync(clubViewModel.Image);
                var club = new Club
                {
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubViewModel.Address.Street,
                        City = clubViewModel.Address.City,
                        State = clubViewModel.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Photo upload failed");
            return View(clubViewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            
            var clubViewModel = new ClubEditViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };

            return View(clubViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ClubEditViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubViewModel);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoservice.DeletePhotoAsync(userClub.Image);
                }
                catch
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubViewModel);
                }
                var photoResult = await _photoservice.AddPhotoAsync(clubViewModel.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubViewModel.AddressId,
                    Address = clubViewModel.Address,
                    ClubCategory = clubViewModel.ClubCategory
                };

                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubViewModel);
            }
        }
    }
}
