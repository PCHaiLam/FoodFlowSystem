using FoodFlowSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FoodFlowSystem.Interceptors
{
    public class TimeInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateAuditFires(context);
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateAuditFires(context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private static void UpdateAuditFires(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
