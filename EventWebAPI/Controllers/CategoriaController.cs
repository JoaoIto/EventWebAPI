using EventWebAPI.Data;
using EventWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        /// <remarks>
        /// Este endpoint retorna uma lista de todas as categorias disponíveis no sistema.
        /// </remarks>
        /// <response code="200">Lista de categorias retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [SwaggerResponse(200, "Lista de Categorias", typeof(IEnumerable<Categoria>))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return Ok(await _context.Categorias.ToListAsync());
        }

        /// <summary>
        /// Retorna uma categoria específica pelo ID.
        /// </summary>
        /// <param name="id">ID da categoria a ser buscada.</param>
        /// <remarks>
        /// Este endpoint retorna uma categoria específica com base no ID fornecido.
        /// </remarks>
        /// <response code="200">Categoria encontrada com sucesso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound(new { Message = "Categoria não encontrada." });
            }

            return Ok(categoria);
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        /// <param name="categoria">Dados da categoria a ser criada.</param>
        /// <remarks>
        /// Este endpoint permite criar uma nova categoria no sistema.
        /// </remarks>
        /// <response code="201">Categoria criada com sucesso.</response>
        /// <response code="400">Erro na validação dos dados fornecidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Categoria>> CreateCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validação extra: verificar se o nome já existe
            if (_context.Categorias.Any(c => c.Nome == categoria.Nome))
            {
                return BadRequest(new { Message = "Categoria com este nome já existe." });
            }

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.CategoriaId }, categoria);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser atualizada.</param>
        /// <param name="categoria">Novos dados para a categoria.</param>
        /// <remarks>
        /// Este endpoint atualiza os dados de uma categoria específica com base no ID fornecido.
        /// </remarks>
        /// <response code="204">Categoria atualizada com sucesso.</response>
        /// <response code="400">Erro na validação dos dados fornecidos.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest(new { Message = "ID fornecido não coincide com o da categoria." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                    return NotFound(new { Message = "Categoria não encontrada." });
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
        /// <remarks>
        /// Este endpoint exclui uma categoria com base no ID fornecido.
        /// </remarks>
        /// <response code="204">Categoria excluída com sucesso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { Message = "Categoria não encontrada." });
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
