using Microsoft.Identity.Client;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;

namespace RunGroupWebApp.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUserClubsAsync()
        {
            var currUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == currUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRacesAsync()
        {
            var currUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == currUser);
            return userRaces.ToList();
        }
    }
}
