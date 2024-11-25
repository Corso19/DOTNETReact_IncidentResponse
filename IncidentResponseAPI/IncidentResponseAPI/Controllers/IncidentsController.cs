using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentsService _incidentsService;

        public IncidentsController(IIncidentsService incidentsService)
        {
            _incidentsService = incidentsService;
        }

        // GET: api/Incidents
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of incidents")]
        public async Task<ActionResult<IEnumerable<IncidentsDto>>> GetIncidents()
        {
            return Ok(await _incidentsService.GetAllAsync());
        }

        // GET: api/Incidents/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets an incident by ID")]
        public async Task<ActionResult<IncidentsDto>> GetIncident(int id)
        {
            var incidentDto = await _incidentsService.GetByIdAsync(id);

            if (incidentDto == null)
            {
                return NotFound();
            }

            return incidentDto;
        }

        // POST: api/Incidents
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new incident")]
        public async Task<ActionResult<IncidentsDto>> PostIncident(IncidentsDto incidentsDto)
        {
            await _incidentsService.AddAsync(incidentsDto);
            return CreatedAtAction(nameof(GetIncident), new { id = incidentsDto.IncidentId }, incidentsDto);
        }

        // PUT: api/Incidents/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing incident")]
        public async Task<IActionResult> PutIncident(int id, IncidentsDto incidentsDto)
        {
            await _incidentsService.UpdateAsync(id, incidentsDto);
            return NoContent();
        }

        // DELETE: api/Incidents/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an incident by ID")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            await _incidentsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
