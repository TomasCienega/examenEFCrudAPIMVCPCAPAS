using examenCrudAPI_CAPAS.Cliente.Services.Contratos;
using examenCrudAPI_CAPAS.Cliente.ViewModel;
using examenCrudAPI_CAPAS.Compartido.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace examenCrudAPI_CAPAS.Cliente.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IDepartamentoService _departamentoService;
        public EmpleadoController(IEmpleadoService empleadoService, IDepartamentoService departamentoService)
        {
            _empleadoService = empleadoService;
            _departamentoService = departamentoService;
        }
        public async Task<IActionResult> Index(int idDep)
        {
            var vm = new EmpleadoDTOVM();
            ViewBag.IdSeleccionado = idDep;
            try
            {
                vm.ListaDepartamento = await _departamentoService.ListarDeptos();
                if (idDep > 0)
                {
                    vm.ListaEmpleado = await _empleadoService.ListaEmpleadosByIdDep(idDep);
                }
                else
                {
                    vm.ListaEmpleado = await _empleadoService.ListaEmpleados();
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                vm.ListaEmpleado = new List<EmpleadoDTO>();
                vm.ListaDepartamento = new List<DepartamentoDTO>();
                Console.WriteLine(ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(EmpleadoDTOVM vm)
        {
            try
            {
                await _empleadoService.GuardarEmpleado(vm.EmpleadoModelReference);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int idEmp)
        {
            var vm = new EmpleadoDTOVM();
            try
            {
                vm.ListaDepartamento = await _departamentoService.ListarDeptos();
                var empleado = await _empleadoService.ObtenerEmpleadoById(idEmp);
                if (empleado.IdEmpleado == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    vm.EmpleadoModelReference = empleado;
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                vm.ListaDepartamento = await _departamentoService.ListarDeptos();
                Console.WriteLine(ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmpleadoDTOVM vm)
        {
            try
            {
                await _empleadoService.EditarEmpleado(vm.EmpleadoModelReference);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int idEmp, int idDep)
        {
            try
            {
                bool _empleadoEliminado = await _empleadoService.EliminarEmpleado(idEmp);
                if (_empleadoEliminado)
                {
                    Console.WriteLine("Eliminado correctamente");
                }
                else
                {
                    Console.WriteLine("No se pudo eliminar");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index", new {idDep = idDep });
        }

        [HttpPost]
        public async Task<IActionResult> Estado(int idEmp, int idDep)
        {
            try
            {
                bool _estadoEmpleado = await _empleadoService.EstadoEmpleado(idEmp);
                if (_estadoEmpleado)
                {
                    Console.WriteLine("Cambio de estado correctamente");
                }
                else
                {
                    Console.WriteLine("No se pudo cambiar el estado");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
