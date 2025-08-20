using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Repositories.Abastract
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametroObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo);
    }
}
