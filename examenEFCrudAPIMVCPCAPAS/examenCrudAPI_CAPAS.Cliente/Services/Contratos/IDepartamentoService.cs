using examenCrudAPI_CAPAS.Compartido.DTOs;

namespace examenCrudAPI_CAPAS.Cliente.Services.Contratos
{
    public interface IDepartamentoService
    {

        Task<List<DepartamentoDTO>> ListarDeptos();

    }
}
