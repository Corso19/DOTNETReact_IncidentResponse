using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services
{
    public class IncidentsService : IIncidentsService
    {
        private readonly IIncidentsRepository _incidentsRepository;

        public IncidentsService(IIncidentsRepository incidentsRepository)
        {
            _incidentsRepository = incidentsRepository;
        }

        public async Task<IEnumerable<IncidentsDto>> GetAllAsync()
        {
            var incidents = await _incidentsRepository.GetAllAsync();
            return incidents.Select(i => new IncidentsDto
            {
                IncidentId = i.IncidentId,
                Title = i.Title,
                Description = i.Description,
                DetectedAt = i.DetectedAt,
                Status = i.Status
            }).ToList();
        }

        public async Task<IncidentsDto> GetByIdAsync(int id)
        {
            var i = await _incidentsRepository.GetByIdAsync(id);
            if (i == null) return null;

            return new IncidentsDto
            {
                IncidentId = i.IncidentId,
                Title = i.Title,
                Description = i.Description,
                DetectedAt = i.DetectedAt,
                Status = i.Status
            };
        }

        public async Task AddAsync(IncidentsDto incidentsDto)
        {
            var incidentsModel = new IncidentsModel
            {
                Title = incidentsDto.Title,
                Description = incidentsDto.Description,
                DetectedAt = incidentsDto.DetectedAt,
                Status = incidentsDto.Status
            };

            await _incidentsRepository.AddAsync(incidentsModel);
        }

        public async Task UpdateAsync(int id, IncidentsDto incidentsDto)
        {
            var incidentsModel = new IncidentsModel
            {
                IncidentId = id,
                Title = incidentsDto.Title,
                Description = incidentsDto.Description,
                DetectedAt = incidentsDto.DetectedAt,
                Status = incidentsDto.Status
            };

            await _incidentsRepository.UpdateAsync(incidentsModel);
        }

        public async Task DeleteAsync(int id)
        {
            await _incidentsRepository.DeleteAsync(id);
        }
    }
}
