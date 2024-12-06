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
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os usuários.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /// <summary>
        /// Retorna um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Usuário específico.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuario">Objeto com os dados do usuário.</param>
        /// <returns>Usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Se os dados fornecidos não forem válidos.</response>
        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="usuario">Objeto com os novos dados do usuário.</param>
        /// <returns>Resposta da atualização.</returns>
        /// <response code="204">Usuário atualizado com sucesso.</response>
        /// <response code="400">Se os dados não forem válidos ou o ID não coincidir.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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
        /// Exclui um usuário específico.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído.</param>
        /// <returns>Resposta da exclusão.</returns>
        /// <response code="204">Usuário excluído com sucesso.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(u => u.UsuarioId == id);
        }
    }
}
