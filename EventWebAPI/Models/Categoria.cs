using System.ComponentModel.DataAnnotations;

namespace EventWebAPI.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }


        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da categoria não pode exceder 50 caracteres.")]
        [MinLength(3, ErrorMessage = "O nome da categoria deve ter pelo menos 3 caracteres.")]
        [RegularExpression(@"^[a-zA-Zà-úÀ-Ú\s]*$", ErrorMessage = "O nome da categoria deve conter apenas letras.")]
        [Display(Name = "Nome da Categoria", Description = "Nome da Categoria.")]
        public string Nome { get; set; }

        [Display(Name = "Lista de Eventos", Description = "Lista de Eventos")]
        public ICollection<Evento> Eventos { get; set; }
    }
}
