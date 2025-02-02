# API de Libros

Una API RESTful para gestionar información de libros, construida con ASP.NET Core, Swagger, autenticación JWT, encriptación de contraseñas y Dapper.

## Tabla de Contenidos

* [Introducción](#introducción)
* [Instalación](#instalación)
* [Uso](#uso)
* [Características](#características)
* [Contribución](#contribución)
* [Licencia](#licencia)

## Introducción

Esta API proporciona una interfaz para administrar libros, incluyendo la creación, lectura, actualización y eliminación de libros, así como la gestión de usuarios y la autenticación mediante tokens JWT.

## Instalación

Sigue estos pasos para configurar la API en tu entorno local usando Visual Studio 2022:

### Requisitos

*   Visual Studio 2022 (con las cargas de trabajo de desarrollo web de ASP.NET y .NET)
*   .NET 8 SDK (se instala automáticamente con Visual Studio)
*   SQL Server (u otro motor de base de datos compatible con Dapper)
*   Postman (u otra herramienta para probar APIs)

### Pasos

1.  Clona el repositorio:
    *   Abre Visual Studio 2022.
    *   Selecciona "Clonar un repositorio".
    *   Ingresa la URL del repositorio: `https://github.com/juan1214s/Libros-Api`
    *   Elige una ubicación local para el repositorio y haz clic en "Clonar".

2.  Abre la solución en Visual Studio:
    *   Una vez clonado, Visual Studio te ofrecerá abrir la solución.  Haz clic en "Abrir solución".  Si no, puedes abrirla manualmente desde la ubicación donde clonaste el repositorio (busca un archivo `.sln`).

3.  Configura la cadena de conexión a la base de datos:
    *   Abre el archivo `appsettings.json` en el Solution Explorer.
    *   Modifica la sección `"ConnectionStrings"` con los datos de tu base de datos SQL Server:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=TuServidor;Database=TuBaseDeDatos;User Id=TuUsuario;Password=TuContraseña;"
    },
    "JwtSettings": {
      "SecretKey": "TuClaveSecretaDe32Bytes", // Genera una clave secreta segura (al menos 32 bytes)
      "TokenExpirationMinutes": 60  // Duración del token en minutos
    }
    ```
    *   **Importante:** Genera una clave secreta segura para `JwtSettings:SecretKey`.  Puedes usar un generador de contraseñas o código.  Esta clave debe ser *confidencial*.

4.  Aplica las migraciones de la base de datos:
    *   Abre la "Consola del Administrador de Paquetes" (Tools > NuGet Package Manager > Package Manager Console).
    *   Asegúrate de que el proyecto predeterminado sea el proyecto de la API.
    *   Ejecuta el siguiente comando:
    ```powershell
    Update-Database
    ```

5.  Ejecuta la API:
    *   Haz clic en el botón "Iniciar" (o presiona F5) en Visual Studio.  Esto compilará y ejecutará la API.  Visual Studio Express suele abrir una ventana del navegador apuntando a `localhost` y un puerto.

## Uso

### Swagger

La API incluye Swagger para la documentación interactiva.  Una vez que la API se está ejecutando, accede a la interfaz en `http://localhost:ElPuerto/swagger` (reemplaza `ElPuerto` con el puerto que Visual Studio asignó a tu aplicación).

### Autenticación

*   **Registro de usuario**: Envía una solicitud POST a `/usuarios` con los datos del nuevo usuario (email, contraseña, etc.).
*   **Inicio de sesión**: Envía una solicitud POST a `/usuarios/login` con el email y la contraseña del usuario. Recibirás un token JWT en la respuesta.
*   **Autorización**: Incluye el token JWT en el encabezado `Authorization` de las solicitudes a los endpoints protegidos (usando el esquema `Bearer`).  Por ejemplo, en Postman:
    *   Key: `Authorization`
    *   Value: `Bearer TuTokenJWT`

### Ejemplos de endpoints

*   **Obtener todos los libros**:
    ```
    GET /libros
    ```
*   **Obtener un libro por ID**:
    ```
    GET /libros/{id}
    ```
*   **Crear un nuevo libro**:
    ```
    POST /libros
    ```

## Características

*   **Autenticación y autorización**: Protección de endpoints mediante tokens JWT.
*   **Encriptación de contraseñas**: Almacenamiento seguro de contraseñas utilizando BCrypt.
*   **Acceso a datos eficiente**: Uso de Dapper para interactuar con la base de datos.
*   **Documentación interactiva**: Swagger para explorar y probar la API.
*   **Validación de datos**: Utilización de Data Annotations para asegurar la integridad de los datos.
*   **Manejo de excepciones**: Middleware para capturar y gestionar excepciones de manera centralizada.

## Contribución

¡Las contribuciones son bienvenidas! Si deseas mejorar la API, corregir errores o agregar nuevas características, por favor, abre un "issue" o envía una solicitud de extracción (pull request).


*   Asegúrate de que la clave secreta JWT (`JwtSettings:SecretKey`) sea segura y se mantenga confidencial.
*   Considera configurar HTTPS para proteger la comunicación entre el cliente y la API.
*   Implementa pruebas unitarias y de integración para garantizar la calidad del código.

¡Espero que este README te sea útil! Recuerda personalizarlo con los detalles específicos de tu API.
