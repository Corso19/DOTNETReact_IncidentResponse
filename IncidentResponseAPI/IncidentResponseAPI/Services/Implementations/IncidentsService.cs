﻿using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations
{
    public class IncidentsService : IIncidentsService
    {
        private readonly IIncidentsRepository _incidentsRepository;

        public IncidentsService(IIncidentsRepository incidentsRepository)
        {
            _incidentsRepository = incidentsRepository;
        }

        public async Task<IEnumerable<IncidentDto>> GetAllAsync()
        {
            var incidents = await _incidentsRepository.GetAllAsync();
            return incidents.Select(i => new IncidentDto
            {
                IncidentId = i.IncidentId,
                Title = i.Title,
                Description = i.Description,
                DetectedAt = i.DetectedAt,
                Status = i.Status
            }).ToList();
        }

        public async Task<IncidentDto> GetByIdAsync(int id)
        {
            var i = await _incidentsRepository.GetByIdAsync(id);
            if (i == null) return null;

            return new IncidentDto
            {
                IncidentId = i.IncidentId,
                Title = i.Title,
                Description = i.Description,
                DetectedAt = i.DetectedAt,
                Status = i.Status
            };
        }

        public async Task AddAsync(IncidentDto incidentDto)
        {
            var incidentsModel = new IncidentsModel
            {
                Title = incidentDto.Title,
                Description = incidentDto.Description,
                DetectedAt = incidentDto.DetectedAt,
                Status = incidentDto.Status
            };

            await _incidentsRepository.AddAsync(incidentsModel);
        }

        public async Task UpdateAsync(int id, IncidentDto incidentDto)
        {
            var incidentsModel = new IncidentsModel
            {
                IncidentId = id,
                Title = incidentDto.Title,
                Description = incidentDto.Description,
                DetectedAt = incidentDto.DetectedAt,
                Status = incidentDto.Status
            };

            await _incidentsRepository.UpdateAsync(incidentsModel);
        }

        public async Task DeleteAsync(int id)
        {
            await _incidentsRepository.DeleteAsync(id);
        }
    }
}
