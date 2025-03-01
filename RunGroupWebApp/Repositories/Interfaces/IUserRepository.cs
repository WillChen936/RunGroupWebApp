using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(string id);
        bool Add(AppUser club);
        bool Update(AppUser club);
        bool Delete(AppUser club);
        bool Save();
    }
}
