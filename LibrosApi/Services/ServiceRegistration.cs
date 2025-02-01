using LibrosApi.Services.Bcrypt;
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
        }
    }
}
