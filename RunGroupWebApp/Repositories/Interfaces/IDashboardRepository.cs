using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Club>> GetAllUserClubsAsync();
        Task<List<Race>> GetAllUserRacesAsync();
    }
}
