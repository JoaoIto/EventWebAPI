using EventWebAPI.Data;
using EventWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os eventos.
        /// </summary>
        /// <returns>Lista de eventos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos.ToListAsync();
        }

        /// <summary>
        /// Retorna um evento específico pelo ID.
        /// </summary>
        /// <param name="id">ID do evento.</param>
        /// <returns>Evento específico.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        /// <summary>
        /// Cria um novo evento.
        /// </summary>
        /// <param name="evento">Objeto com os dados do evento.</param>
        /// <returns>Evento criado.</returns>
        /// <response code="201">Evento criado com sucesso.</response>
        /// <response code="400">Se os dados fornecidos não forem válidos.</response>
        [HttpPost]
        public async Task<ActionResult<Evento>> CreateEvento([FromBody] Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
        }

        /// <summary>
        /// Atualiza um evento existente.
        /// </summary>
        /// <param name="id">ID do evento a ser atualizado.</param>
        /// <param name="evento">Objeto com os novos dados do evento.</param>
        /// <returns>Resposta da atualização.</returns>
        /// <response code="204">Evento atualizado com sucesso.</response>
        /// <response code="400">Se os dados não forem válidos ou o ID não coincidir.</response>
        /// <response code="404">Se o evento não for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] Evento evento)
        {
            if (id != evento.EventoId)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Exclui um evento específico.
        /// </summary>
        /// <param name="id">ID do evento a ser excluído.</param>
        /// <returns>Resposta da exclusão.</returns>
        /// <response code="204">Evento excluído com sucesso.</response>
        /// <response code="404">Se o evento não for encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.EventoId == id);
        }
    }
}
