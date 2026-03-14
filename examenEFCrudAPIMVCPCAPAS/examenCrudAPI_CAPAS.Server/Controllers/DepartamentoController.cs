using examenCrudAPI_CAPAS.Compartido.DTOs;
using examenCrudAPI_CAPAS.Server.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examenCrudAPI_CAPAS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly ExamenEfcrudApimvcpcapasContext _context;
        public DepartamentoController(ExamenEfcrudApimvcpcapasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaDepartamento")]
        public async Task<ActionResult<List<DepartamentoDTO>>> ListaDepartamentos()
        {
            try
            {
                var _listaDTO = new List<DepartamentoDTO>();
                var _listaBD = await _context.Departamentos.ToListAsync();

                foreach (var deps in _listaBD)
                {
                    _listaDTO.Add(new DepartamentoDTO
                    {
                        IdDepartamento = deps.IdDepartamento,
                        NombreDepartamento = deps.NombreDepartamento
                    });
                }
                return Ok(_listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
