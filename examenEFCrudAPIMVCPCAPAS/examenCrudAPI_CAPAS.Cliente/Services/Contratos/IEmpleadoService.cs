using examenCrudAPI_CAPAS.Compartido.DTOs;
using System;

namespace examenCrudAPI_CAPAS.Cliente.Services.Contratos
{
    public interface IEmpleadoService
    {

        Task<List<EmpleadoDTO>> ListaEmpleados();
        Task<List<EmpleadoDTO>> ListaEmpleadosByIdDep(int idDep);
        Task<EmpleadoDTO> ObtenerEmpleadoById(int idEmp);
        Task<EmpleadoDTO> GuardarEmpleado(EmpleadoDTO empdto);
        Task<EmpleadoDTO> EditarEmpleado(EmpleadoDTO empdto);
        Task<bool> EliminarEmpleado(int idEmp);
        Task<bool> EstadoEmpleado(int idEmp);

    }
}

