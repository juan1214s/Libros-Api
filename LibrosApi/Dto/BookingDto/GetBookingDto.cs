using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto.BookingDto
{
    public class GetBookingDto
    {
        public int Id { get; set; }       // Debe coincidir con p.id
        public int IdLibro { get; set; }   // Debe coincidir con p.id_libro
        public string TituloLibro { get; set; } // Debe coincidir con l.titulo AS TituloLibro
        public int IdUsuario { get; set; } // Debe coincidir con p.id_usuario
        public string NombreUsuario { get; set; } // Debe coincidir con u.nombre AS NombreUsuario
        public DateTime FechaPrestamo { get; set; } // Debe coincidir con p.fecha_prestamo
        public DateTime? FechaDevolucion { get; set; } // Debe coincidir con p.fecha_devolucion
    }
}
