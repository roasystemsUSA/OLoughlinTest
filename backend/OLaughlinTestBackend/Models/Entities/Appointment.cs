namespace Models.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }          // UNIQUEIDENTIFIER
        public Guid CustomerId { get; set; }  // FK hacia Customers
        public DateTime DateTime { get; set; } // DATETIME2
        public string Status { get; set; } = "scheduled"; // NOT NULL, default lógico

        // Relación: una cita pertenece a un cliente
        public Customer? Customer { get; set; }
    }
}
