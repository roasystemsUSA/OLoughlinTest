using Models.Entities;
using Repositories;

namespace Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            return await _appointmentRepository.AddAsync(appointment);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            return await _appointmentRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }
        public async Task<Appointment?> GetByIdAsync(string id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }
        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            return await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}