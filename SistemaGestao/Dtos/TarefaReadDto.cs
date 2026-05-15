using SistemaGestao.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestao.Dtos
{
    public class TarefaReadDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public Status? Status { get; set; }
    }
}
