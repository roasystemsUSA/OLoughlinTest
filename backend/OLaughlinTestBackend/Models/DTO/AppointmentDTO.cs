using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Status { get; set; } = string.Empty;

        // Campo adicional para mostrar el nombre del cliente
        public string CustomerName { get; set; } = string.Empty;
    }
}
