using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Repositories.Abastract;
using ManejoPresupuesto.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositoriosTipoCuentas _repositoriosTipoCuentas;
        private readonly IServicioUsuarios _servicioUsuarios;

        public TiposCuentasController(IRepositoriosTipoCuentas repositoriosTipoCuentas, IServicioUsuarios servicioUsuarios)
        {
            _repositoriosTipoCuentas = repositoriosTipoCuentas;
            _servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositoriosTipoCuentas.Obtener(usuarioId);

            return View(tiposCuentas);

        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var existeTipoCuenta = await _repositoriosTipoCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (existeTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El nombre {tipoCuenta.Nombre} ya existe.");

                return View(tipoCuenta);
            }

            await _repositoriosTipoCuentas.Crear(tipoCuenta);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositoriosTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositoriosTipoCuentas.ObtenerPorId(tipoCuenta.Id, tipoCuenta.UsuarioId);

            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("No encontrado", "Home");
            }

            await _repositoriosTipoCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositoriosTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositoriosTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _repositoriosTipoCuentas.Borrar(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await _repositoriosTipoCuentas.Existe(nombre, usuarioId);

            if (existeTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositoriosTipoCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if (idsTiposCuentasNoPertenecenAlUsuario.Count > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor, indice) => 
                new TipoCuenta() { Id = valor, Orden = indice +1 }).AsEnumerable();

            await _repositoriosTipoCuentas.Ordenar(tiposCuentasOrdenados);

            return Ok();
        }
    }
}
