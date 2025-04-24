using FoodFlowSystem.Entities.Payment;

namespace FoodFlowSystem.Repositories.Payment
{
    public interface IPaymentRepository : IBaseRepository<PaymentEntity>
    {
        Task<PaymentEntity> GetPendingDepositPaymentsByOrderId(int orderId);
    }
}
