using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly LivoraDbContext _context;

        public PaymentRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Billing)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Billing)
                    .ThenInclude(b => b.Contract)
                .Include(p => p.PaymentMethod)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBillingAsync(int billingId)
        {
            return await _context.Payments
                .Include(p => p.Billing)
                .Include(p => p.PaymentMethod)
                .Where(p => p.BillingId == billingId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidForBillingAsync(int billingId)
        {
            return await _context.Payments
                .Where(p => p.BillingId == billingId && p.IsActive)
                .SumAsync(p => p.AmountPaid);
        }
    }
}