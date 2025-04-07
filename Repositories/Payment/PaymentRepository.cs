using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Payment;

namespace FoodFlowSystem.Repositories.Payment
{
    public class PaymentRepository : BaseRepository<PaymentEntity>, IPaymentRepository
    {
        public PaymentRepository(MssqlDbContext context) : base(context)
        {
        }
    }
}
