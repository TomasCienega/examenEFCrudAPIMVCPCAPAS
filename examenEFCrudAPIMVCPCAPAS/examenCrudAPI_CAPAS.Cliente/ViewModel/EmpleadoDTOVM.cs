using examenCrudAPI_CAPAS.Compartido.DTOs;

namespace examenCrudAPI_CAPAS.Cliente.ViewModel
{
    public class EmpleadoDTOVM
    {
        public List<EmpleadoDTO> ListaEmpleado { get; set; } = new();
        public EmpleadoDTO EmpleadoModelReference { get; set; }= new();
        public List<DepartamentoDTO> ListaDepartamento { get; set; } = new();
    }
}
