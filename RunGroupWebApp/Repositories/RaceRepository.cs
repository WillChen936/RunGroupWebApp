using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories.Interfaces;

namespace RunGroupWebApp.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Race>> GetAllAsync()
        {
            return await _context.Races.ToListAsync();
        }

        public Task<Race> GetByIdAsync(int id)
        {
            return _context.Races.Include(a => a.Address).FirstAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetByCityAsync(string city)
        {
            return await _context.Races.Include(a => a.Address).Where(c => c.Address.City == city).ToListAsync();
        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
