using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories
{
    public interface IRecommendationsRepository
    {
        Task<IEnumerable<RecommendationsModel>> GetAllAsync();
        Task<RecommendationsModel> GetByIdAsync(int id);
        Task AddAsync(RecommendationsModel recommendationsModel);
        Task UpdateAsync(RecommendationsModel recommendationsModel);
        Task DeleteAsync(int id);
    }
}
