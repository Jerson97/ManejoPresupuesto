using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Repositories.Abastract
{
    public interface IRepositorioUsuarios
    {
        Task Actualizar(Usuario usuario);
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
}
