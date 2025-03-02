using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;
using RunGroupWebApp.Services.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoservice _photoService;
        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoservice photoservice)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoservice;
        }
        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboardRepository.GetAllUserClubsAsync();
            var userRaces = await _dashboardRepository.GetAllUserRacesAsync();
            var dashboardViewModel = new DashboardViewModel
            {
                Clubs = userClubs,
                Races = userRaces
            };
            return View(dashboardViewModel);
        }
        public async Task<IActionResult> EditUserProfile()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserByIdAsync(currUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel
            {
                Id = currUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editViewModel);
            }
            var user = await _dashboardRepository.GetUserByIdAsync(editViewModel.Id);
            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editViewModel.Image);
                MapUserEdit(user, editViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(editViewModel.Image);
                MapUserEdit(user, editViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editViewModel, ImageUploadResult photoResult)
        {
            user.Id = editViewModel.Id;
            user.Pace = editViewModel.Pace;
            user.Mileage = editViewModel.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editViewModel.City;
            user.State = editViewModel.State;
        }
    }
}
