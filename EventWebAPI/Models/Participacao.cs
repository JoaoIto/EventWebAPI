namespace EventWebAPI.Models
{
    public class Participacao
    {
        public int ParticipacaoId { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public bool Confirmado { get; set; }
    }
}
