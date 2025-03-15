namespace FoodFlowSystem.DTOs.Responses
{
    public class UserResponse
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}