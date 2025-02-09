using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllAsync();
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Race>> GetByCityAsync(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
