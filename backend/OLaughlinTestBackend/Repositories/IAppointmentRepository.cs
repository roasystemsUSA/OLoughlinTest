using Models.Entities;

namespace Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(string id);
        Task<Appointment> AddAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(string id);
    }
}
