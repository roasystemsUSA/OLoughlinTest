using Models.Entities;

namespace Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(string id);
        Task<Customer> AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<Appointment>> GetAppointmentsAsync(string customerId);
    }
}
