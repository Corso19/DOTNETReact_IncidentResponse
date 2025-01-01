using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations
{
    public class IncidentEventService : IIncidentEventService
    {
        private readonly IIncidentEventRepository _incidentEventRepository;

        public IncidentEventService(IIncidentEventRepository incidentEventRepository)
        {
            _incidentEventRepository = incidentEventRepository;
        }

        public async Task<IEnumerable<IncidentEventDto>> GetAllAsync()
        {
            var incidentEvents = await _incidentEventRepository.GetAllAsync();
            return incidentEvents.Select(ie => new IncidentEventDto
            {
                IncidentId = ie.IncidentId,
                EventId = ie.EventId
            }).ToList();
        }

        public async Task<IncidentEventDto> GetByIdAsync(int incidentId, int eventId)
        {
            var ie = await _incidentEventRepository.GetByIdAsync(incidentId, eventId);
            if (ie == null) return null;

            return new IncidentEventDto
            {
                IncidentId = ie.IncidentId,
                EventId = ie.EventId
            };
        }

        public async Task AddAsync(IncidentEventDto incidentEventDto)
        {
            var incidentEventModel = new IncidentEventModel
            {
                IncidentId = incidentEventDto.IncidentId,
                EventId = incidentEventDto.EventId
            };

            await _incidentEventRepository.AddAsync(incidentEventModel);
        }

        public async Task DeleteAsync(int incidentId, int eventId)
        {
            await _incidentEventRepository.DeleteAsync(incidentId, eventId);
        }
    }
}



