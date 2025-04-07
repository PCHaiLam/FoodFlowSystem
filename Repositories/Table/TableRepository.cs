using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Table;

namespace FoodFlowSystem.Repositories.Table
{
    public class TableRepository : BaseRepository<TableEntity>, ITableRepository
    {
        public TableRepository(MssqlDbContext context) : base(context)
        {
        }
    }
}
