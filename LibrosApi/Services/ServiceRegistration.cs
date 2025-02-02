using LibrosApi.Dto.AuthorsDto;
using LibrosApi.Services.Authors;
using LibrosApi.Services.Bcrypt;
using LibrosApi.Services.Book;
using LibrosApi.Services.Booking;
using LibrosApi.Services.Books;
using LibrosApi.Services.Categories;
using LibrosApi.Services.Login;
using LibrosApi.Services.Token;
using LibrosApi.Services.User;

namespace LibrosApi.Services
{
    public static class ServiceRegistration
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            //Bcrypt
            services.AddScoped<BcryptService>();

            //Usuarios
            services.AddScoped<CreateUserService>();
            services.AddScoped<DeleteUserService>();
            services.AddScoped<GetUserByIdService>();
            services.AddScoped<UpdateUserService>();

            //Categorias
            services.AddScoped<CreateCategoriesService>();
            services.AddScoped<GetCategoriesService>();
            services.AddScoped<DeleteCategoriesService>();
            services.AddScoped<UpdateCategoriesService>();
            services.AddScoped<GetByIdCategoryService>();

            //Autores
            services.AddScoped<CreateAuthorService>();
            services.AddScoped<DeleteAuthorService>();
            services.AddScoped<GetAuthorsService>();
            services.AddScoped<UpdateAuthorService>();
            services.AddScoped<GetAuthorByIdService>();

            //Book
            services.AddScoped<CreateBookService>();
            services.AddScoped<GetBooksService>();
            services.AddScoped<DeleteBookService>();
            services.AddScoped<UpdateBookService>();
            services.AddScoped<GetBookByIdService>();

            //Reservas
            services.AddScoped<CreateBookingService>();
            services.AddScoped<GetBookingService>();

            //Token
            services.AddScoped<TokenService>();

            //Login
            services.AddScoped<LoginService>();
        }
    }
}
