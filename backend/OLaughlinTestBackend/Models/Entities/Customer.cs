namespace Models.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }   // UNIQUEIDENTIFIER en SQL
        public string Name { get; set; } = string.Empty; // NOT NULL
        public string? Email { get; set; }               // NULL permitido

        // Relación: un cliente puede tener muchas citas
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
