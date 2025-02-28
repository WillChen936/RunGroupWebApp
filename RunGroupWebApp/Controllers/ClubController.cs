using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;
using RunGroupWebApp.Services;
using RunGroupWebApp.Services.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoservice _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoservice photoservice, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoservice;
            _httpContextAccessor = httpContextAccessor;
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
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel {AppUserId = currUserId };
            return View(createClubViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel createClubViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(createClubViewModel.Image);
                var club = new Club
                {
                    AppUserId = createClubViewModel.AppUserId,
                    Title = createClubViewModel.Title,
                    Description = createClubViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = createClubViewModel.Address.Street,
                        City = createClubViewModel.Address.City,
                        State = createClubViewModel.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Photo upload failed");
            return View(createClubViewModel);
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
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubViewModel.Image);
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
        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            if (!string.IsNullOrEmpty(clubDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(clubDetails.Image);
            }

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
