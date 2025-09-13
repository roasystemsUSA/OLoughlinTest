using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(string id);
        Task<Appointment> AddAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(string id);
    }
}
