using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Payment;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Payment
{
    public class PaymentRepository : BaseRepository<PaymentEntity>, IPaymentRepository
    {
        public PaymentRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task<PaymentEntity> GetPendingDepositPaymentsByOrderId(int orderId)
        {
            var payments = await _dbContext.Payments
                .Include(p => p.Invoice)
                .Where(p =>
                    p.Status == "Pending" &&
                    p.PaymentType == "deposit" &&
                    p.Invoice != null &&
                    p.Invoice.OrderID == orderId)
                .FirstOrDefaultAsync();

            return payments;
        }
    }
}
