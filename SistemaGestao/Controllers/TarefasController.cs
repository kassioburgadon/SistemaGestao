using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestao.Data;
using SistemaGestao.Dtos;
using SistemaGestao.Models;

namespace SistemaGestao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly SistemaGestaoContext _context;

        public TarefasController(SistemaGestaoContext context)
        {
            _context = context;
        }

        // GET: api/Tarefas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefa()
        {
            if (_context.Tarefa == null)
            {
                return NotFound();
            }
            return await _context.Tarefa.ToListAsync();
        }

        // GET: api/Tarefas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTarefa(Guid id)
        {
            if (_context.Tarefa == null)
            {
                return NotFound();
            }
            var tarefa = await _context.Tarefa.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return tarefa;
        }

        // PUT: api/Tarefas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(Guid id, TarefaUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tarefa = await _context.Tarefa.FindAsync(id);
            if (tarefa == null)
                return NotFound();

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.DataVencimento = dto.DataVencimento;
            tarefa.Status = dto.Status.Value;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(tarefa);

        }

        // POST: api/Tarefas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tarefa>> PostTarefa(TarefaCreateDto dto)
        {
            if (_context.Tarefa == null)
            {
                return Problem("Entity set 'SistemaGestaoContext.Tarefa'  is null.");
            }

            var tarefa = new Tarefa
            {
                Id = Guid.NewGuid(),
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataVencimento = dto.DataVencimento ?? DateTime.MinValue,
                Status = dto.Status
            };
            _context.Tarefa.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarefa", new { id = tarefa.Id }, tarefa.Id);
        }

        // DELETE: api/Tarefas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(Guid id)
        {
            if (_context.Tarefa == null)
            {
                return NotFound();
            }
            var tarefa = await _context.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefa.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarefaExists(Guid id)
        {
            return (_context.Tarefa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
