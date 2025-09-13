using Models.Entities;
using Repositories;

namespace Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        public CustomerService(ICustomerRepository customerRepository, IAppointmentRepository appointmentRepository)
        {
            _customerRepository = customerRepository;
            _appointmentRepository = appointmentRepository;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            return await _customerRepository.AddAsync(customer);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            return await _customerRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }
        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(string customerId)
        {
            return await _customerRepository.GetAppointmentsAsync(customerId);
        }
        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await _customerRepository.UpdateAsync(customer);
        }
    }
}
