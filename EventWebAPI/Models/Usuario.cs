namespace EventWebAPI.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public ICollection<Comentario> Comentarios { get; set; }
        public ICollection<Participacao> Participacoes { get; set; }
    }
}
