using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestao.Data;
using SistemaGestao.Dtos;
using SistemaGestao.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        /// <summary>
        /// Obtém todas as tarefas cadastradas no sistema
        /// </summary>
        /// <returns>Retorna uma lista de tarefas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefa()
        {
            if (_context.Tarefa == null)
            {
                return NotFound();
            }
            return await _context.Tarefa.ToListAsync();
        }

        /// <summary>
        /// Obtém uma tarefa específica ou lista filtrada.
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <param name="status">Filtro por status.</param>
        /// <param name="dataVencimento">Filtro por data de vencimento.</param>
        /// <returns>Retorna uma tarefa ou lista de tarefas</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TarefaReadDto>>> GetTarefas(
        Guid? id,
        [FromQuery] Status? status,
        [FromQuery] DateTime? dataVencimento)
        {

            if (_context.Tarefa == null)
            {
                return NotFound();
            }
            var tarefa = _context.Tarefa.AsQueryable();

            if (tarefa == null)
            {
                return NotFound();
            }

            if (status != null)
                tarefa = tarefa.Where(t => t.Status == status.Value);

            if (dataVencimento != null)
                tarefa = tarefa.Where(t => t.DataVencimento.Value.Date == dataVencimento.Value.Date);

            var dto = await tarefa
                 .Where(t => t.Id == id)
                .Select(t => new TarefaReadDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    DataVencimento = t.DataVencimento,
                    Status = t.Status
                })
                .ToListAsync();

            if (dto == null || !dto.Any())
                return NotFound(); // lista vazia


            return Ok(dto);
        }

        /// <summary>
        /// Atualiza uma tarefa existente no sistema.
        /// </summary>
        /// <param name="id">Id da tarefa que será atualizada.</param>
        /// <param name="dto">Objeto contendo os novos dados da tarefa.</param>
        /// <returns>
        /// Retorna a tarefa atualizada em caso de sucesso (200 OK).  
        /// Retorna erro de validação se os dados forem inválidos (400 Bad Request).  
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TarefaUpdateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Cria uma nova tarefa no sistema.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados necessários para criar a tarefa.</param>
        /// <returns>
        /// Retorna a tarefa criada em caso de sucesso (201 Created).  
        /// Retorna erro de validação se os dados forem inválidos (400 Bad Request).  
        /// Retorna Erro se o contexto de tarefas estiver indisponível.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(TarefaReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

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

        /// <summary>
        /// Exclui uma tarefa existente do sistema.
        /// </summary>
        /// <param name="id">Id da tarefa que será excluída.</param>
        /// <returns>
        /// Retorna uma mensagem de confirmação e o Id da tarefa excluída em caso de sucesso (200 OK).  
        /// Retorna NotFound se a tarefa não existir ou se o contexto estiver indisponível (404 Not Found).
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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

            return Ok(new { Mensagem = "Tarefa deletado com sucesso", Id = id });
        }

        private bool TarefaExists(Guid id)
        {
            return (_context.Tarefa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
