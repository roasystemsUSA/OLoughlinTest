using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<bool> UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer == null)
            {
                return false;
            }
            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(string customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.Id.Equals(customerId));
            return customer?.Appointments ?? Enumerable.Empty<Appointment>();
        }
    }
}