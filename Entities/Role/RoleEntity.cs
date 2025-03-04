using FoodFlowSystem.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FoodFlowSystem.Entities.Role
{
    public class RoleEntity : BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
