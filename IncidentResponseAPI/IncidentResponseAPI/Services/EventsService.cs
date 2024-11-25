﻿using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;

namespace IncidentResponseAPI.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsInterface _eventsRepository;

        public EventsService(IEventsInterface eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<IEnumerable<EventsDto>> GetAllAsync()
        {
            var events = await _eventsRepository.GetAllAsync();
            return events.Select(e => new EventsDto
            {
                EventId = e.EventId,
                SensorId = e.SensorId,
                EventDataJson = e.EventDataJson,
                Timestamp = e.Timestamp,
                isProcessed = e.isProcessed
            }).ToList();
        }

        public async Task<EventsDto> GetByIdAsync(int id)
        {
            var e = await _eventsRepository.GetByIdAsync(id);
            if (e == null) return null;

            return new EventsDto
            {
                EventId = e.EventId,
                SensorId = e.SensorId,
                EventDataJson = e.EventDataJson,
                Timestamp = e.Timestamp,
                isProcessed = e.isProcessed
            };
        }

        public async Task AddAsync(EventsDto eventDto)
        {
            var eventsModel = new EventsModel
            {
                SensorId = eventDto.SensorId,
                EventDataJson = eventDto.EventDataJson,
                Timestamp = eventDto.Timestamp,
                isProcessed = eventDto.isProcessed
            };

            await _eventsRepository.AddAsync(eventsModel);
        }

        public async Task UpdateAsync(int id, EventsDto eventDto)
        {
            var eventsModel = new EventsModel
            {
                EventId = id,
                SensorId = eventDto.SensorId,
                EventDataJson = eventDto.EventDataJson,
                Timestamp = eventDto.Timestamp,
                isProcessed = eventDto.isProcessed
            };

            await _eventsRepository.UpdateAsync(eventsModel);
        }

        public async Task DeleteAsync(int id)
        {
            await _eventsRepository.DeleteAsync(id);
        }
    }
}




