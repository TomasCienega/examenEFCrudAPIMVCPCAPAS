using examenCrudAPI_CAPAS.Cliente.Services.Contratos;
using examenCrudAPI_CAPAS.Compartido.DTOs;

namespace examenCrudAPI_CAPAS.Cliente.Services.Implementaciones
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly HttpClient _httpClient;
        public DepartamentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<DepartamentoDTO>> ListarDeptos()
        {
            var _listaDepsDTO = await _httpClient.GetFromJsonAsync<List<DepartamentoDTO>>("api/Departamento/ListaDepartamento") ?? new List<DepartamentoDTO>();
            return _listaDepsDTO;
        }
    }
}
