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
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as categorias.
        /// </summary>
        /// <returns>Lista de categorias.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        /// <summary>
        /// Retorna uma categoria específica pelo ID.
        /// </summary>
        /// <param name="id">ID da categoria.</param>
        /// <returns>Categoria específica.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        /// <param name="categoria">Objeto com os dados da categoria.</param>
        /// <returns>Categoria criada.</returns>
        /// <response code="201">Categoria criada com sucesso.</response>
        /// <response code="400">Se os dados fornecidos não forem válidos.</response>
        [HttpPost]
        public async Task<ActionResult<Categoria>> CreateCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.CategoriaId }, categoria);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser atualizada.</param>
        /// <param name="categoria">Objeto com os novos dados da categoria.</param>
        /// <returns>Resposta da atualização.</returns>
        /// <response code="204">Categoria atualizada com sucesso.</response>
        /// <response code="400">Se os dados não forem válidos ou o ID não coincidir.</response>
        /// <response code="404">Se a categoria não for encontrada.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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
        /// Exclui uma categoria específica.
        /// </summary>
        /// <param name="id">ID da categoria a ser excluída.</param>
        /// <returns>Resposta da exclusão.</returns>
        /// <response code="204">Categoria excluída com sucesso.</response>
        /// <response code="404">Se a categoria não for encontrada.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }
    }
}
