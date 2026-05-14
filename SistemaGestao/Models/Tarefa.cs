using System.Text.Json.Serialization;

namespace SistemaGestao.Models
{
    public class Tarefa
    {
        private string titulo;
        private Status status;

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get => titulo; set => titulo = value; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public Status? Status { get; set; }
    }

    public enum Status
    {
        Pendente = 0,
        EmProgresso = 1,
        Concluida = 2,
    }
}
