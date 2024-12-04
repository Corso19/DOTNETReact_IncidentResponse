using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Repositories.Implementations
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly IncidentResponseContext _context;

        public RecommendationsRepository(IncidentResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecommendationsModel>> GetAllAsync()
        {
            return await _context.RecommendationsModel.ToListAsync();
        }

        public async Task<RecommendationsModel> GetByIdAsync(int id)
        {
            return await _context.RecommendationsModel.FindAsync(id);
        }

        public async Task AddAsync(RecommendationsModel recommendationsModel)
        {
            _context.RecommendationsModel.Add(recommendationsModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecommendationsModel recommendationsModel)
        {
            _context.Entry(recommendationsModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recommendationsModel = await _context.RecommendationsModel.FindAsync(id);
            if (recommendationsModel != null)
            {
                _context.RecommendationsModel.Remove(recommendationsModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
