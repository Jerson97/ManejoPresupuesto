using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Repositories.Abastract;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Repositories.Implementation
{
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionString;
        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Usuarios(Email, EmailNormalizado, PasswordHash)
                VALUES (@Email, @EmailNormalizado, @PasswordHash);
                SELECT SCOPE_IDENTITY();
                ", usuario);

            return id;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios where EmailNormalizado = @emailNormalizado",
                new { emailNormalizado });
        }
    }
}
