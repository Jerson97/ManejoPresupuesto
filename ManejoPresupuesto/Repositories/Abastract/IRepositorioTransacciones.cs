using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Repositories.Abastract
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
    }
}
