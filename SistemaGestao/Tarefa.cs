using System.Text.Json.Serialization;

namespace SistemaGestao
{
    public class Tarefa
    {
        private string titulo;
        private string descricao;
        private DateTime dataVencimento;
        private Status status;

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get => titulo; set => titulo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public DateTime DataVencimento { get => dataVencimento; set => dataVencimento = value; }
        public Status Status { get => status; set => status = value; }
    }

    public enum Status
    {
        Pendente = 0,
        EmProgresso = 1,
        Concluida = 2,
    }
}
