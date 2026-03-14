using examenCrudAPI_CAPAS.Compartido.DTOs;
using examenCrudAPI_CAPAS.Server.Context;
using examenCrudAPI_CAPAS.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examenCrudAPI_CAPAS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly ExamenEfcrudApimvcpcapasContext _context;
        public EmpleadoController(ExamenEfcrudApimvcpcapasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaEmpleado")]
        public async Task<ActionResult<List<EmpleadoDTO>>> ListaEmpleados()
        {
            try
            {
                var _listaDTO = new List<EmpleadoDTO>();
                var _listaBD = await _context.Empleados.
                    Include(tD => tD.IdDepartamentoNavigation).
                    OrderByDescending(a => a.Activo).
                    ToListAsync();
                foreach (var emps in _listaBD)
                {
                    _listaDTO.Add(new EmpleadoDTO
                    {
                        IdEmpleado = emps.IdEmpleado,
                        NombreEmpleado = emps.NombreEmpleado,
                        IdDepartamento = emps.IdDepartamento,
                        NombreDepartamento = emps.IdDepartamentoNavigation.NombreDepartamento,
                        Activo = emps.Activo
                    });
                }
                return Ok(_listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ListaDepartamentobyId/{idDep}")]
        public async Task<ActionResult<List<EmpleadoDTO>>> ListaDepartamentosByIdDep(int idDep)
        {
            try
            {
                var _listaDTO = new List<EmpleadoDTO>();
                var _listaBD = await _context.Empleados.
                    FromSqlRaw("exec sp_ListarEmpleadoPorIdDep {0}", idDep == 0 ? null : idDep).
                    OrderByDescending(a => a.Activo).
                    ToListAsync();
                foreach (var emps in _listaBD)
                {
                    await _context.Entry(emps).Reference(e => e.IdDepartamentoNavigation).LoadAsync();

                    _listaDTO.Add(new EmpleadoDTO
                    {
                        IdEmpleado = emps.IdEmpleado,
                        NombreEmpleado = emps.NombreEmpleado,
                        IdDepartamento = emps.IdDepartamento,
                        NombreDepartamento = emps.IdDepartamentoNavigation.NombreDepartamento,
                        Activo = emps.Activo
                    });
                }
                return Ok(_listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerEmpleado/{idEmp}")]
        public async Task<ActionResult<EmpleadoDTO>> ObtenerEmpleado(int idEmp)
        {
            try
            {
                var _empleadoDTO = new EmpleadoDTO();
                var _empleadoBD = await _context.Empleados.
                    Include(tD => tD.IdDepartamentoNavigation).
                    Where(e => e.IdEmpleado == idEmp).
                    FirstOrDefaultAsync();

                if (_empleadoBD == null)
                    return NotFound("No encontré al empleado");

                _empleadoDTO.IdEmpleado = idEmp;
                _empleadoDTO.NombreEmpleado = _empleadoBD.NombreEmpleado;
                _empleadoDTO.IdDepartamento = _empleadoBD.IdDepartamento;
                _empleadoDTO.NombreDepartamento = _empleadoBD.IdDepartamentoNavigation.NombreDepartamento;
                _empleadoDTO.Activo = _empleadoBD.Activo;

                return Ok(_empleadoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GuardarEmpleado")]
        public async Task<ActionResult<EmpleadoDTO>> GuardarEmpleado(EmpleadoDTO empdto)
        {
            try
            {
                var _empleadoBD = new Empleado
                {
                    NombreEmpleado = empdto.NombreEmpleado,
                    IdDepartamento = empdto.IdDepartamento
                };

                await _context.Empleados.AddAsync(_empleadoBD);
                await _context.SaveChangesAsync();
                return Ok(_empleadoBD);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EditarEmpleado")]
        public async Task<ActionResult<EmpleadoDTO>> EditarEmpleado(EmpleadoDTO empdto)
        {
            try
            {
                var _empleadoBD = await _context.Empleados.
                    Where(e => e.IdEmpleado == empdto.IdEmpleado).
                    FirstOrDefaultAsync();
                if (_empleadoBD != null)
                {
                    _empleadoBD.NombreEmpleado = empdto.NombreEmpleado;
                    _empleadoBD.IdDepartamento = empdto.IdDepartamento;
                    _empleadoBD.Activo = empdto.Activo;

                    _context.Empleados.Update(_empleadoBD);
                    await _context.SaveChangesAsync();

                    return Ok(_empleadoBD);
                }
                else
                {
                    return BadRequest($"El usuario con el id {empdto.IdEmpleado} no existe");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarEmpleado/{idEmp}")]
        public async Task<ActionResult<bool>> EliminarEmpleado(int idEmp)
        {
            try
            {

                var _empleadoBD = await _context.Empleados.
                    Where(e => e.IdEmpleado == idEmp).
                    FirstOrDefaultAsync();

                if (_empleadoBD == null)
                {
                    return NotFound(false);
                }
                else
                {
                    _context.Empleados.Remove(_empleadoBD);
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, false);
            }
        }

        [HttpPut]
        [Route("EditarEstado/{idEmp}")]
        public async Task<ActionResult<bool>> EstadoEmpleado(int idEmp)
        {
            try
            {
                // 1. Validar que el ID sea válido antes de ir a la DB
                if (idEmp <= 0) return BadRequest(false);

                // 2. Verificar existencia
                var existe = await _context.Empleados.AnyAsync(e => e.IdEmpleado == idEmp);
                if (!existe) return NotFound(false);

                // 3. Ejecutar SP
                await _context.Database.ExecuteSqlRawAsync("exec sp_EstadoEmpleado {0}", idEmp);

                // 4. Retornar confirmación pura
                return Ok(true);
            }
            catch (Exception)
            {
                // Si algo falla en la base de datos, mandamos false
                return StatusCode(500, false);
            }
        }
    }
}
