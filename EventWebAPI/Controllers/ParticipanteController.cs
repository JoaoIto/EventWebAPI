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
    public class ParticipanteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipanteController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os participantes.
        /// </summary>
        /// <returns>Lista de participantes.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participacao>>> GetParticipantes()
        {
            return await _context.Participacoes.ToListAsync();
        }

        /// <summary>
        /// Retorna um participante específico pelo ID.
        /// </summary>
        /// <param name="id">ID do participante.</param>
        /// <returns>Participante específico.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Participacao>> GetParticipante(int id)
        {
            var participante = await _context.Participacoes.FindAsync(id);

            if (participante == null)
            {
                return NotFound();
            }

            return participante;
        }

        /// <summary>
        /// Cria um novo participante.
        /// </summary>
        /// <param name="participante">Objeto com os dados do participante.</param>
        /// <returns>Participante criado.</returns>
        /// <response code="201">Participante criado com sucesso.</response>
        /// <response code="400">Se os dados fornecidos não forem válidos.</response>
        [HttpPost]
        public async Task<ActionResult<Participacao>> CreateParticipante([FromBody] Participacao participante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Participacoes.Add(participante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParticipante), new { id = participante.ParticipacaoId }, participante);
        }

        /// <summary>
        /// Atualiza um participante existente.
        /// </summary>
        /// <param name="id">ID do participante a ser atualizado.</param>
        /// <param name="participante">Objeto com os novos dados do participante.</param>
        /// <returns>Resposta da atualização.</returns>
        /// <response code="204">Participante atualizado com sucesso.</response>
        /// <response code="400">Se os dados não forem válidos ou o ID não coincidir.</response>
        /// <response code="404">Se o participante não for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipante(int id, [FromBody] Participacao participante)
        {
            if (id != participante.ParticipacaoId)
            {
                return BadRequest();
            }

            _context.Entry(participante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipanteExists(id))
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
        /// Exclui um participante específico.
        /// </summary>
        /// <param name="id">ID do participante a ser excluído.</param>
        /// <returns>Resposta da exclusão.</returns>
        /// <response code="204">Participante excluído com sucesso.</response>
        /// <response code="404">Se o participante não for encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipante(int id)
        {
            var participante = await _context.Participacoes.FindAsync(id);
            if (participante == null)
            {
                return NotFound();
            }

            _context.Participacoes.Remove(participante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipanteExists(int id)
        {
            return _context.Participacoes.Any(p => p.ParticipacaoId == id);
        }
    }
}
