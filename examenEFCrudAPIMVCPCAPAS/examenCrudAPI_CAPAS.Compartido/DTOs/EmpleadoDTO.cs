using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examenCrudAPI_CAPAS.Compartido.DTOs
{
    public class EmpleadoDTO
    {
        public int IdEmpleado { get; set; }

        public string NombreEmpleado { get; set; } = null!;

        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; } = string.Empty;

        public bool? Activo { get; set; }
    }
}
