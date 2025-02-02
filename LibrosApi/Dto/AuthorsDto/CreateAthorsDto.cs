using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto.AuthorsDto
{
    public class CreateAthorsDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }
    }
}
