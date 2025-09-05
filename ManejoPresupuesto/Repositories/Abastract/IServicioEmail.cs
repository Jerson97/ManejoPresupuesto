namespace ManejoPresupuesto.Repositories.Abastract
{
    public interface IServicioEmail
    {
        Task EnviarEmailCambioPassword(string receptor, string enlace);
    }
}
