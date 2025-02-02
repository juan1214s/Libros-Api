using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto.BookingDto
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "La fecha de préstamo es obligatoria.")]
        public DateTime FechaPrestamo { get; set; }

        public DateTime FechaDevolucion { get; set; }
    }
}
