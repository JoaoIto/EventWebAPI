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
    public class ComentarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComentarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os comentários.
        /// </summary>
        /// <returns>Lista de comentários.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios()
        {
            return await _context.Comentarios.ToListAsync();
        }

        /// <summary>
        /// Retorna um comentário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do comentário.</param>
        /// <returns>Comentário específico.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        /// <summary>
        /// Cria um novo comentário.
        /// </summary>
        /// <param name="comentario">Objeto com os dados do comentário.</param>
        /// <returns>Comentário criado.</returns>
        /// <response code="201">Comentário criado com sucesso.</response>
        /// <response code="400">Se os dados fornecidos não forem válidos.</response>
        [HttpPost]
        public async Task<ActionResult<Comentario>> CreateComentario([FromBody] Comentario comentario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComentario), new { id = comentario.ComentarioId }, comentario);
        }

        /// <summary>
        /// Atualiza um comentário existente.
        /// </summary>
        /// <param name="id">ID do comentário a ser atualizado.</param>
        /// <param name="comentario">Objeto com os novos dados do comentário.</param>
        /// <returns>Resposta da atualização.</returns>
        /// <response code="204">Comentário atualizado com sucesso.</response>
        /// <response code="400">Se os dados não forem válidos ou o ID não coincidir.</response>
        /// <response code="404">Se o comentário não for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComentario(int id, [FromBody] Comentario comentario)
        {
            if (id != comentario.ComentarioId)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
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
        /// Exclui um comentário específico.
        /// </summary>
        /// <param name="id">ID do comentário a ser excluído.</param>
        /// <returns>Resposta da exclusão.</returns>
        /// <response code="204">Comentário excluído com sucesso.</response>
        /// <response code="404">Se o comentário não for encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(c => c.ComentarioId == id);
        }
    }
}
