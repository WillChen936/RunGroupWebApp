using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Club>> GetAllUserClubsAsync();
        Task<List<Race>> GetAllUserRacesAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
