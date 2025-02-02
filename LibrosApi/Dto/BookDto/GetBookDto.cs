namespace LibrosApi.Dto.BookDto
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string NombreAutor { get; set; }
        public string NombreCategoria { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
