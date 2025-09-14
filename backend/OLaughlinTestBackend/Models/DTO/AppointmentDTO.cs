using Models.Entities;

namespace Models.DTO
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Status { get; set; } = string.Empty;

        // Campo adicional para mostrar el nombre del cliente
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public static AppointmentDTO FromEntity(Entities.Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id,
                DateTime = appointment.DateTime,
                Status = appointment.Status,
                CustomerName = appointment.Customer?.Name ?? string.Empty,
                CustomerEmail = appointment.Customer?.Email ?? string.Empty
            };
        }
        public static List<AppointmentDTO> FromEntityList(List<Entities.Appointment> appointments)
        {
            return appointments.Select(a => FromEntity(a)).ToList();
        }

        public static List<Appointment> ToEntitiesList(List<AppointmentDTO> appointmentDTOs)
        {
            return appointmentDTOs.Select(dto => dto.ToEntity()).ToList();
        }

        public Entities.Appointment ToEntity()
        {
            return new Entities.Appointment
            {
                Id = this.Id,
                DateTime = this.DateTime,
                Status = this.Status                
            };
        }
    }
}
