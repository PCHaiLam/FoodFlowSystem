using FoodFlowSystem.Entities.AuditLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace FoodFlowSystem.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AddAuditLogs(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            AddAuditLogs(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditLogs(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            if (!entries.Any()) return;

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers.UserAgent.ToString();

            var auditLogs = new List<AuditLogEntity>();

            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType().Name;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();

                var auditLog = new AuditLogEntity
                {
                    Action = entry.State.ToString(),
                    TableName = entityType,
                    RecordID = primaryKey,
                    OldValue = entry.State == EntityState.Added ? null : JsonSerializer.Serialize(entry.OriginalValues.ToObject()),
                    NewValue = entry.State == EntityState.Deleted ? null : JsonSerializer.Serialize(entry.CurrentValues.ToObject()),
                    IPAddress = ip ?? "Unknown",
                    UserAgent = userAgent ?? "Unknown",
                    UserID = userId != null ? int.Parse(userId) : null,
                    CreatedAt = DateTime.UtcNow
                };

                auditLogs.Add(auditLog);
            }

            if (auditLogs.Any())
            {
                context.Set<AuditLogEntity>().AddRange(auditLogs);
            }
        }
    }
}