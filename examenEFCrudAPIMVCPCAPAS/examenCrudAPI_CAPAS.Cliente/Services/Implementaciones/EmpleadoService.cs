using examenCrudAPI_CAPAS.Cliente.Services.Contratos;
using examenCrudAPI_CAPAS.Compartido.DTOs;
using System;

namespace examenCrudAPI_CAPAS.Cliente.Services.Implementaciones
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly HttpClient _httpClient;
        public EmpleadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmpleadoDTO>> ListaEmpleados()
        {
            var _listaEmp = await _httpClient.GetFromJsonAsync<List<EmpleadoDTO>>("api/Empleado/ListaEmpleado") ?? new List<EmpleadoDTO>();
            return _listaEmp;
        }

        public async Task<List<EmpleadoDTO>> ListaEmpleadosByIdDep(int idDep)
        {
            var _listaEmpByIdDep = await _httpClient.GetFromJsonAsync<List<EmpleadoDTO>>($"api/Empleado/ListaDepartamentobyId/{idDep}") ?? new List<EmpleadoDTO>();
            return _listaEmpByIdDep;
        }

        public async Task<EmpleadoDTO> ObtenerEmpleadoById(int idEmp)
        {
                var _obtenerById = await _httpClient.GetFromJsonAsync<EmpleadoDTO>($"api/Empleado/ObtenerEmpleado/{idEmp}") ?? new EmpleadoDTO();
                return _obtenerById;
        }

        public async Task<EmpleadoDTO> GuardarEmpleado(EmpleadoDTO empdto)
        {
            var _empleadoGuardado = await _httpClient.PostAsJsonAsync("api/Empleado/GuardarEmpleado", empdto);
            if (_empleadoGuardado.IsSuccessStatusCode)
            {
                return await _empleadoGuardado.Content.ReadFromJsonAsync<EmpleadoDTO>() ?? new EmpleadoDTO();
            }
            return new EmpleadoDTO();
        }

        public async Task<EmpleadoDTO> EditarEmpleado(EmpleadoDTO empdto)
        {
            var _empleadoEditado = await _httpClient.PutAsJsonAsync("api/Empleado/EditarEmpleado", empdto);
            if (_empleadoEditado.IsSuccessStatusCode)
            {
                return await _empleadoEditado.Content.ReadFromJsonAsync<EmpleadoDTO>() ?? new EmpleadoDTO();
            }
            return new EmpleadoDTO();
        }

        public async Task<bool> EliminarEmpleado(int idEmp)
        {
            var _empleadoEliminado = await _httpClient.DeleteAsync($"api/Empleado/EliminarEmpleado/{idEmp}");
            return _empleadoEliminado.IsSuccessStatusCode;
        }

        public async Task<bool> EstadoEmpleado(int idEmp)
        {
            var _estadoEmpleado = await _httpClient.PutAsync($"api/Empleado/EditarEstado/{idEmp}",null);
            return _estadoEmpleado.IsSuccessStatusCode;
        }

    }
}
