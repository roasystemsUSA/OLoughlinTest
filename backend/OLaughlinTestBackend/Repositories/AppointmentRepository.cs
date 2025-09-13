using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Appointment?> GetByIdAsync(string id)
        {
            return await _context.Appointments.FindAsync(id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointment.Id);
            if (existingAppointment == null)
            {
                return false;
            }
            _context.Entry(existingAppointment).CurrentValues.SetValues(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return false;
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
