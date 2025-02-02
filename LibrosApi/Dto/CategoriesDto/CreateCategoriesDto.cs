using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto.CategoriesDto
{
    public class CreateCategoriesDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }
    }
}
