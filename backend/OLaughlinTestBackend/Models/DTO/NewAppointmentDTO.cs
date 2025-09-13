using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class NewAppointmentDTO
    {
        public Guid CustomerId { get; set; }  // FK hacia Customers
        public DateTime DateTime { get; set; } // DATETIME2
        public string Status { get; set; } = "scheduled"; // NOT NULL, default lógico
    }
}
