using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto.BookDto
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "El titulo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El titulo no puede tener más de 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        public int? IdAutor { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria.")]
        public int? IdCategoria { get; set; }

        [Required(ErrorMessage = "El año de publicacion es obligatorio.")]
        public DateTime FechaPublicacion { get; set; } 
    }
}
