using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.OAuth;

namespace FoodFlowSystem.Repositories.OAuth
{
    public class OAuthRepository : BaseRepository<OAuthEntity>, IOAuthRepository
    {
        public OAuthRepository(MssqlDbContext context) : base(context)
        {
        }
    }
}
