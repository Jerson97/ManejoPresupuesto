using System.Net.Mail;
using System.Net;
using ManejoPresupuesto.Repositories.Abastract;

namespace ManejoPresupuesto.Repositories.Implementation
{
    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration configuration;

        public ServicioEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarEmailCambioPassword(string receptor, string enlace)
        {
            var email = configuration.GetValue<string>("CONFIGURACIONES_GMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_GMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_GMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_GMAIL:PUERTO");

            var cliente = new SmtpClient(host, puerto);
            cliente.EnableSsl = true;
            cliente.UseDefaultCredentials = false;

            cliente.Credentials = new NetworkCredential(email, password);
            var emisor = email;
            var tema = "¿Ha olvidado su contraseña?";

            var contenidoHTML = $@"Saludos,

            Este mensaje le llega porque usted ha solicitado un cambio de contraseña. Si esta solicitud no fue hecha por usted, puede ignorar este mensaje.

            Para cambiar su contraseña, haga click en el siguiente enlace:

            {enlace}

            Atentamente,
            Equipo Manejo Presupuesto";

            var mensajeEmail = new MailMessage(emisor, receptor, tema, contenidoHTML);
            await cliente.SendMailAsync(mensajeEmail);
        }
    }
}
