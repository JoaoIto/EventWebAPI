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
        /// <remarks>
        /// Este endpoint retorna uma lista de todos os participantes registrados no sistema.
        /// </remarks>
        /// <response code="200">Lista de participantes retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Participacao>>> GetParticipantes()
        {
            return Ok(await _context.Participacoes.ToListAsync());
        }

        /// <summary>
        /// Retorna um participante específico pelo ID.
        /// </summary>
        /// <param name="id">ID do participante a ser buscado.</param>
        /// <remarks>
        /// Este endpoint retorna os dados de um participante com base no ID fornecido.
        /// </remarks>
        /// <response code="200">Participante encontrado com sucesso.</response>
        /// <response code="404">Participante não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Participacao>> GetParticipante(int id)
        {
            var participante = await _context.Participacoes.FindAsync(id);

            if (participante == null)
            {
                return NotFound(new { Message = "Participante não encontrado." });
            }

            return Ok(participante);
        }

        /// <summary>
        /// Cria um novo participante.
        /// </summary>
        /// <param name="participante">Objeto contendo os dados do participante.</param>
        /// <remarks>
        /// Este endpoint cria um novo participante no sistema.
        /// </remarks>
        /// <response code="201">Participante criado com sucesso.</response>
        /// <response code="400">Erro na validação dos dados fornecidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <param name="participante">Dados atualizados do participante.</param>
        /// <remarks>
        /// Este endpoint permite atualizar as informações de um participante existente.
        /// </remarks>
        /// <response code="204">Participante atualizado com sucesso.</response>
        /// <response code="400">Erro na validação dos dados fornecidos ou ID não corresponde.</response>
        /// <response code="404">Participante não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateParticipante(int id, [FromBody] Participacao participante)
        {
            if (id != participante.ParticipacaoId)
            {
                return BadRequest(new { Message = "ID fornecido não corresponde ao ID do participante." });
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
                    return NotFound(new { Message = "Participante não encontrado." });
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
        /// <remarks>
        /// Este endpoint exclui um participante com base no ID fornecido.
        /// </remarks>
        /// <response code="204">Participante excluído com sucesso.</response>
        /// <response code="404">Participante não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParticipante(int id)
        {
            var participante = await _context.Participacoes.FindAsync(id);
            if (participante == null)
            {
                return NotFound(new { Message = "Participante não encontrado." });
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
