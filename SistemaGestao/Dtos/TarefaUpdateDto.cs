using SistemaGestao.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestao.Dtos
{
    public class TarefaUpdateDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        [Required(ErrorMessage = "O status é obrigatório.")]
        public Status? Status { get; set; }
    }
}
