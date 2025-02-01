namespace LibrosApi.Exceptions
{
    // Clase base para excepciones personalizadas, que hereda de Exception.
    // Permite agregar un código de estado HTTP (por defecto 500 para errores internos del servidor).
    public class AppExceptions : Exception
    {
        public int StatusCode { get; }  // Propiedad que guarda el código de estado HTTP

        // Constructor que recibe el mensaje de error y un código de estado opcional (por defecto 500).
        public AppExceptions(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;  // Establece el código de estado HTTP
        }
    }

    // Excepción personalizada para indicar que un usuario ya existe.
    // Se utiliza un código de estado HTTP 409 (Conflict).
    public class ResourceAlreadyExistsException : AppExceptions
    {
        // Constructor que recibe el mensaje de error y lo pasa al constructor base con el código de estado 409.
        public ResourceAlreadyExistsException(string message) : base(message, 409) { }
    }

    // Excepción personalizada para indicar que el acceso no es permitido a un recurso.
    // Se utiliza un código de estado HTTP 403 (Forbidden).
    public class AccessNotReSource : AppExceptions
    {
        // Constructor que recibe el mensaje de error y lo pasa al constructor base con el código de estado 403.
        public AccessNotReSource(string message) : base(message, 403) { }
    }

    // Excepción personalizada para indicar que un recurso no se encuentra.
    // Se utiliza el código de estado HTTP 404 (Not Found).
    public class ResourceNotFoundException : AppExceptions
    {
        // Constructor que recibe un mensaje de error y lo pasa al constructor base con el código de estado 404.
        public ResourceNotFoundException(string message) : base(message, 404) { }
    }
}
