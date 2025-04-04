﻿using FoodFlowSystem.Entities.AuditLog;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Entities.OAuth;
using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Reservation;
using FoodFlowSystem.Entities.Role;

namespace FoodFlowSystem.Entities.User
{
    public class UserEntity : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public int RoleID { get; set; }
        public RoleEntity Role { get; set; }
        public ICollection<ReservationEntity> Reservations { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
        public ICollection<AuditLogEntity> AuditLogs { get; set; }
        public ICollection<FeedbackEntity> Feedbacks { get; set; }
        public ICollection<OAuthEntity> OAuths { get; set; }

    }
}
